// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Diagnostics
{
    using System;

    /// <summary>
    /// The logger ex.
    /// </summary>
    public static class LoggerEx
    {
        /// <summary>
        /// Log error.
        /// </summary>
        /// <param name="this">
        /// The this.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="@this"/> or <paramref name="exception"/> is null.
        /// </exception>
        public static void LogErrorException(this ILogger @this, Exception exception)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("@this");
            }

            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            @this.Error(exception.ToString());
        }

        /// <summary>
        /// The log debug exception.
        /// </summary>
        /// <param name="this">
        /// The this.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="@this"/> or <paramref name="exception"/> is null.
        /// </exception>
        public static void LogDebugException(this ILogger @this, Exception exception)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("@this");
            }

            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            @this.Debug(exception.ToString());
        }
    }
}