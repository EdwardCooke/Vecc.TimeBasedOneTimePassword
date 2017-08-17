using System;
using System.Globalization;
using System.Security.Cryptography;
using Vecc.Base32;

namespace Vecc.TimeBasedOneTimePassword.Internal
{
    public class Totp : ITotp
    {
        private readonly IBase32 _base32;
        private readonly IDateTimeProvider _dateTimeProvider;

        public Totp(IBase32 base32, IDateTimeProvider dateTimeProvider)
        {
            this._base32 = base32;
            this._dateTimeProvider = dateTimeProvider;
        }

        public string GenerateRandomKey(int length = 8)
        {
            var entropy = new byte[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropy);
            }

            var result = this._base32.Encode(entropy);
            return result;
        }

        public string GetCode(string key)
        {
            var decodedKey = this._base32.DecodeToBytes(key);
            var result = this.GetTotpHash(this._dateTimeProvider.UtcNow, decodedKey);

            return result;
        }

        public bool ValidateCode(string code, string key, int timeTolerance)
        {
            if (timeTolerance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(timeTolerance), "Must be >= 0.");
            }

            var minTolerance = timeTolerance * -30;
            var maxTolerance = timeTolerance * 30;

            var now = this._dateTimeProvider.UtcNow;
            var decodedKey = this._base32.DecodeToBytes(key);

            var result = this.Validate(now, 0, code, decodedKey); //this will be the most common

            if (!result && timeTolerance != 0)
            {
                for (var interval = minTolerance; interval <= maxTolerance; interval += 30)
                {
                    result = this.Validate(now, interval, code, decodedKey);
                    if (result)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        private string GetTotpHash(DateTime time, byte[] key)
        {
            var iterationNumber = (long)Math.Floor((time - this._dateTimeProvider.Epoch).TotalSeconds / 30);
            var hash = this.GetHash(iterationNumber, key);
            var result = this.GetPasswordFromHash(hash);
            return result;
        }

        private bool Validate(DateTime time, int secondsOffset, string code, byte[] key)
        {
            var thisCode = this.GetTotpHash(time.AddSeconds(secondsOffset), key);
            var result = code == thisCode;
            return result;
        }

        private byte[] GetHash(long iterationNumber, byte[] key)
        {
            using (var hmac = new HMACSHA1(key))
            {
                var text = new byte[8];
                for (var i = text.Length - 1; i >= 0; i--)
                {
                    text[i] = (byte)(iterationNumber & 0xff);
                    iterationNumber >>= 8;
                }
                var result = hmac.ComputeHash(text);
                return result;
            }
        }

        protected virtual string GetPasswordFromHash(byte[] hash)
        {
            var offset = hash[hash.Length - 1] & 0x0F;

            var binary = ((hash[offset] & 0x7F) << 24) |
                         ((hash[offset + 1] & 0xFF) << 16) |
                         ((hash[offset + 2] & 0xFF) << 8) |
                         (hash[offset + 3] & 0xFF);

            var otp = binary % 1000000;
            var result = otp.ToString(CultureInfo.InvariantCulture);

            return result;
        }
    }
}
