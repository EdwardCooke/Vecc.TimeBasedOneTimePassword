namespace Vecc.TimeBasedOneTimePassword
{
    /// <summary>
    /// Time base one time password generator and verification
    /// </summary>
    public interface ITotp
    {
        /// <summary>
        /// Generates a base32 encoded random key for use with the authenticator type apps
        /// </summary>
        /// <param name="length">The length of the key. Defaults to 8 bytes.</param>
        /// <returns>Base32 encoded random key.</returns>
        string GenerateRandomKey(int length = 8);

        /// <summary>
        /// Verifies the code is valid for the current time and specified base32 encoded key.
        /// </summary>
        /// <param name="code">Code to validate</param>
        /// <param name="key">Base32 encoded key</param>
        /// <param name="timeTolerance">Number of 30 second intervals to allow for code.</param>
        /// <returns>True if valid, otherwise false.</returns>
        bool ValidateCode(string code, string key, int timeTolerance = 1);

        /// <summary>
        /// Generate the code based on the current time and specified base32 encoded key.
        /// </summary>
        /// <param name="key">Base32 encoded key</param>
        /// <returns>The generate code</returns>
        string GetCode(string key);
    }
}
