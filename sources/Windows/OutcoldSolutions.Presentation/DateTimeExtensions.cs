// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// Extensions methods for <see cref="DateTime"/> class.
    /// </summary>
    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixBaseTime = new DateTime(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// Convert timestamp from Unix File Time (timestamp from 1/1/1970) to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="fileTime">Unix file time stamp (in milliseconds).</param>
        /// <returns>The date time object.</returns>
        public static DateTime FromUnixFileTime(double fileTime)
        {
            return UnixBaseTime.AddMilliseconds(fileTime);
        }

        /// <summary>
        /// Convert <see cref="DateTime"/> to Unix file timestamp (timestamp from 1/1/1970).
        /// </summary>
        /// <param name="this">The date time.</param>
        /// <returns>The timestamp in milliseconds.</returns>
        public static double ToUnixFileTime(this DateTime @this)
        {
            return (@this - UnixBaseTime).TotalMilliseconds;
        }
    }
}