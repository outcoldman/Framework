// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Diagnostics
{
    using System;

    /// <summary>
    /// The LogWriter interface.
    /// </summary>
    public interface ILogWriter
    {
        /// <summary>
        /// Gets a value indicating whether is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Log message.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void Log(DateTime dateTime, LogLevel level, string context, string message, params object[] parameters);

        /// <summary>
        /// Log message.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void Log(DateTime dateTime, LogLevel level, string context, Exception exception, string message, params object[] parameters);
    }
}