// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Presentation.Diagnostics
{
    using System;

    /// <summary>
    /// The log level.
    /// </summary>
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The only error.
        /// </summary>
        OnlyError = 1,

        /// <summary>
        /// The error.
        /// </summary>
        Error = OnlyError,

        /// <summary>
        /// The only warning.
        /// </summary>
        OnlyWarning = 2,

        /// <summary>
        /// The warning.
        /// </summary>
        Warning = OnlyWarning | Error,

        /// <summary>
        /// The only debug.
        /// </summary>
        OnlyDebug = 3,

        /// <summary>
        /// The debug.
        /// </summary>
        Debug = OnlyDebug | Warning,

        /// <summary>
        /// The only info.
        /// </summary>
        OnlyInfo = 4,

        /// <summary>
        /// The info.
        /// </summary>
        Info = OnlyInfo | Debug,
    }
}