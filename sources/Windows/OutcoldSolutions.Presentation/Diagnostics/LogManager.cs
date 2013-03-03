// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Diagnostics
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    /// <summary>
    /// The log manager.
    /// </summary>
    internal class LogManager : ILogManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager"/> class.
        /// </summary>
        public LogManager()
        {
            this.Writers = new ConcurrentDictionary<Type, ILogWriter>();
        }

        /// <inheritdoc />
        public ConcurrentDictionary<Type, ILogWriter> Writers { get; private set; }

        /// <inheritdoc />
        public LogLevel LogLevel { get; set; }

        internal bool IsInfoEnabled
        {
            get { return (this.LogLevel & LogLevel.OnlyInfo) == LogLevel.OnlyInfo; }
        }

        internal bool IsDebugEnabled
        {
            get { return (this.LogLevel & LogLevel.OnlyDebug) == LogLevel.OnlyDebug; }
        }

        internal bool IsWarningEnabled
        {
            get { return (this.LogLevel & LogLevel.OnlyWarning) == LogLevel.OnlyWarning; }
        }

        internal bool IsErrorEnabled
        {
            get { return (this.LogLevel & LogLevel.OnlyError) == LogLevel.OnlyError; }
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string context)
        {
            return new Logger(context, this);
        }

        internal void Info(string context, string message, params object[] parameters)
        {
            if (this.IsInfoEnabled)
            {
                this.Log("Info", context, message, parameters);
            }
        }

        internal void Debug(string context, string message, params object[] parameters)
        {
            if (this.IsDebugEnabled)
            {
                this.Log("Debug", context, message, parameters);
            }
        }

        internal void Warning(string context, string message, params object[] parameters)
        {
            if (this.IsWarningEnabled)
            {
                this.Log("Warning", context, message, parameters);
            }
        }

        internal void Error(string context, string message, params object[] parameters)
        {
            if (this.IsErrorEnabled)
            {
                this.Log("Error", context, message, parameters);
            }
        }

        private void Log(string level, string context, string message, params object[] parameters)
        {
            DateTime dateTime = DateTime.Now;

            Task.Factory.StartNew(
                () =>
                    {
                        var enumerator = this.Writers.GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current.Value.IsEnabled)
                            {
                                try
                                {
                                    enumerator.Current.Value.Log(dateTime, level, context, message, parameters);
                                }
                                catch
                                {
                                }
                            }
                        }
                    });
        }
    }
}