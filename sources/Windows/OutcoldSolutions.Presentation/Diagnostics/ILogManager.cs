// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Presentation.Diagnostics
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// The LogManager interface.
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Gets the writers.
        /// </summary>
        ConcurrentDictionary<Type, ILogWriter> Writers { get; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        LogLevel LogLevel { get; set; }

        /// <summary>
        /// The create logger.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="ILogger"/>.
        /// </returns>
        ILogger CreateLogger(string context);
    }
}