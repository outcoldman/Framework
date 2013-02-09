// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Presentation.Diagnostics
{
    using System.Diagnostics;

    /// <summary>
    /// The logger.
    /// </summary>
    public class Logger : ILogger
    {
        private readonly string context;
        private readonly LogManager logManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        public Logger(string context, LogManager logManager)
        {
            this.context = context;
            this.logManager = logManager;
        }

        /// <inheritdoc/>
        public bool IsInfoEnabled
        {
            get { return Debugger.IsAttached || this.logManager.IsInfoEnabled; }
        }

        /// <inheritdoc/>
        public bool IsDebugEnabled
        {
            get { return Debugger.IsAttached || this.logManager.IsDebugEnabled; }
        }

        /// <inheritdoc/>
        public bool IsWarningEnabled
        {
            get { return Debugger.IsAttached || this.logManager.IsWarningEnabled; }
        }

        /// <inheritdoc/>
        public bool IsErrorEnabled
        {
            get { return Debugger.IsAttached || this.logManager.IsErrorEnabled; }
        }

        /// <inheritdoc/>
        public void Info(string message, params object[] parameters)
        {
            this.logManager.Info(this.context, message, parameters);
        }

        /// <inheritdoc/>
        public void Debug(string message, params object[] parameters)
        {
            this.logManager.Debug(this.context, message, parameters);
        }

        /// <inheritdoc/>
        public void Warning(string message, params object[] parameters)
        {
            this.logManager.Warning(this.context, message, parameters);
        }

        /// <inheritdoc/>
        public void Error(string message, params object[] parameters)
        {
            this.logManager.Error(this.context, message, parameters);
        }
    }
}