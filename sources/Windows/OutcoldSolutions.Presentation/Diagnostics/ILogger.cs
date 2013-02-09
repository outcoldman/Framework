// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Presentation.Diagnostics
{
    /// <summary>
    /// The Logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets a value indicating whether is info enabled.
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether is debug enabled.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether is warning enabled.
        /// </summary>
        bool IsWarningEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether is error enabled.
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Log info message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void Info(string message, params object[] parameters);

        /// <summary>
        /// Log debug message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void Debug(string message, params object[] parameters);

        /// <summary>
        /// Log warning message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void Warning(string message, params object[] parameters);

        /// <summary>
        /// Log error message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void Error(string message, params object[] parameters);
    }
}