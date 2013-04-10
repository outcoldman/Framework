// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    using System;

    /// <summary>
    /// The PopupView interface.
    /// </summary>
    public interface IPopupView : IView
    {
        /// <summary>
        /// Closed event.
        /// </summary>
        event EventHandler<EventArgs> Closed;

        /// <summary>
        /// The close.
        /// </summary>
        void Close();

        /// <summary>
        /// The close.
        /// </summary>
        void Close(EventArgs eventArgs);
    }
}