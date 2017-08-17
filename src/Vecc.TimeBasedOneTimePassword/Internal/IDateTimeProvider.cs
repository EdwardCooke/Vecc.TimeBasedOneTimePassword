using System;

namespace Vecc.TimeBasedOneTimePassword.Internal
{
    /// <summary>
    /// Used to get dates and times.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Current time
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Unix epoch, January 1, 1970 12:00AM
        /// </summary>
        DateTime Epoch { get; }
    }
}
