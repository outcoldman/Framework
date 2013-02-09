// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Diagnostics
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The debug log writer.
    /// </summary>
    public class DebugLogWriter : ILogWriter
    {
        /// <inheritdoc />
        public bool IsEnabled
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public void Log(DateTime dateTime, string level, string context, string messageFormat, params object[] parameters)
        {
            string message;

            if (parameters.Length == 0)
            {
                message = messageFormat;
            }
            else
            {
                message = string.Format(messageFormat, parameters);
            }

            Debug.WriteLine("{0:o}: {1} - {2}:: {3}", dateTime, level, context, message);
        }
    }
}