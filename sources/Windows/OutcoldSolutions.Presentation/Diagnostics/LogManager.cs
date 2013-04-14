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
            get { return LogLevel.Info >= this.LogLevel; }
        }

        internal bool IsDebugEnabled
        {
            get { return LogLevel.Debug >= this.LogLevel; }
        }

        internal bool IsWarningEnabled
        {
            get { return LogLevel.Warning >= this.LogLevel; }
        }

        internal bool IsErrorEnabled
        {
            get { return LogLevel.Error >= this.LogLevel; }
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
                this.Log(LogLevel.Info, context, message, parameters);
            }
        }

        internal void Debug(string context, string message, params object[] parameters)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LogLevel.Debug, context, message, parameters);
            }
        }

        internal void Warning(string context, string message, params object[] parameters)
        {
            if (this.IsWarningEnabled)
            {
                this.Log(LogLevel.Warning, context, message, parameters);
            }
        }

        internal void Error(string context, string message, params object[] parameters)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LogLevel.Error, context, message, parameters);
            }
        }

        internal void Info(string context, Exception exception, string message, params object[] parameters)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LogLevel.Info, context, exception, message, parameters);
            }
        }

        internal void Debug(string context, Exception exception, string message, params object[] parameters)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LogLevel.Debug, context, exception, message, parameters);
            }
        }

        internal void Warning(string context, Exception exception, string message, params object[] parameters)
        {
            if (this.IsWarningEnabled)
            {
                this.Log(LogLevel.Warning, context, exception, message, parameters);
            }
        }

        internal void Error(string context, Exception exception, string message, params object[] parameters)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LogLevel.Error, context, exception, message, parameters);
            }
        }

        private void Log(LogLevel level, string context, string message, params object[] parameters)
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

        private void Log(LogLevel level, string context, Exception exception, string message, params object[] parameters)
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
                                enumerator.Current.Value.Log(dateTime, level, context, exception, message, parameters);
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