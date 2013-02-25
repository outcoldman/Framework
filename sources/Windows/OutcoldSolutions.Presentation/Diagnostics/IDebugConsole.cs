// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Diagnostics
{
    /// <summary>
    /// The DebugConsole interface.
    /// </summary>
    public interface IDebugConsole
    {
        /// <summary>
        /// Write line to debug console.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void WriteLine(string message);
    }
}