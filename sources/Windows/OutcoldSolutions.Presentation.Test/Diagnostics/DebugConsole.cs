// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Test.Diagnostics
{
    using System.Diagnostics;

    using OutcoldSolutions.Diagnostics;

    public class DebugConsole : IDebugConsole
    {
        public void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }
    }
}