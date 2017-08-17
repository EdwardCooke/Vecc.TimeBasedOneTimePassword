using System;

namespace Vecc.TimeBasedOneTimePassword.Internal
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1);

        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Epoch => _epoch;
    }
}
