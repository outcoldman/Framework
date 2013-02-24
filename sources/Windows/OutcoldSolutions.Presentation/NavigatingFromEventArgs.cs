// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The navigating from event args.
    /// </summary>
    public class NavigatingFromEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigatingFromEventArgs"/> class.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        public NavigatingFromEventArgs(IDictionary<string, object> state)
        {
            this.State = state;
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        public IDictionary<string, object> State { get; private set; }
    }
}