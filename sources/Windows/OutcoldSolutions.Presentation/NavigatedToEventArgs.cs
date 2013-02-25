// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;

    using OutcoldSolutions.Views;

    /// <summary>
    /// The navigated to event args.
    /// </summary>
    public class NavigatedToEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigatedToEventArgs"/> class.
        /// </summary>
        /// <param name="view">
        /// The view.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="isBack">
        /// The is back.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="view"/> is null.
        /// </exception>
        public NavigatedToEventArgs(
            IView view,
            IDictionary<string, object> state, 
            object parameter, 
            bool isBack)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            this.View = view;
            this.State = state;
            this.Parameter = parameter;
            this.IsNavigationBack = isBack;
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        public IView View { get; private set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        public IDictionary<string, object> State { get; private set; }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        public object Parameter { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is navigation back.
        /// </summary>
        public bool IsNavigationBack { get; private set; }
    }
}