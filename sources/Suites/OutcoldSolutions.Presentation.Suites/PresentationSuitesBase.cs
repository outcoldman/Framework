// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Suites
{
    using System.Diagnostics;

    using OutcoldSolutions.Diagnostics;

    public class PresentationSuitesBase : SuitesBase
    {
        public PresentationSuitesBase()
            : base(new DebugConsole())
        {
        }

        private class DebugConsole : IDebugConsole
        {
            public void WriteLine(string message)
            {
                Debug.WriteLine(message);
            }
        }
    }
}