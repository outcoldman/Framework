// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Diagnostics
{
    using System;

    /// <summary>
    /// The debug log writer.
    /// </summary>
    public class DebugLogWriter : ILogWriter
    {
        private readonly IDependencyResolverContainer container;
        private readonly IDebugConsole debugConsole;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogWriter"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public DebugLogWriter(IDependencyResolverContainer container)
        {
            this.container = container;
            if (this.container.IsRegistered<IDebugConsole>())
            {
                this.debugConsole = this.container.Resolve<IDebugConsole>();
            }
        }

        /// <inheritdoc />
        public bool IsEnabled
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public void Log(DateTime dateTime, LogLevel level, string context, string messageFormat, params object[] parameters)
        {
            if (this.debugConsole == null)
            {
                return;
            }

            string message;

            if (parameters.Length == 0)
            {
                message = messageFormat;
            }
            else
            {
                message = string.Format(messageFormat, parameters);
            }

            this.debugConsole.WriteLine(string.Format("{0:o}: {1} - {2}:: {3}", dateTime, level, context, message));
        }

        /// <inheritdoc />
        public void Log(DateTime dateTime, LogLevel level, string context, Exception exception, string messageFormat, params object[] parameters)
        {
            if (this.debugConsole == null)
            {
                return;
            }

            string message;

            if (parameters.Length == 0)
            {
                message = messageFormat;
            }
            else
            {
                message = string.Format(messageFormat, parameters);
            }

            this.debugConsole.WriteLine(string.Format("{0:o}: {1} - {2}:: {3}", dateTime, level, context, message));
            this.debugConsole.WriteLine(string.Format("\t {0}", exception));
        }
    }
}