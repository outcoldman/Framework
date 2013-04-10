// --------------------------------------------------------------------------------------------------------------------
// OutcoldSolutions (http://outcoldmansolutions.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    using System;
    using System.Diagnostics;

    using Windows.UI.Xaml.Controls.Primitives;

    /// <summary>
    /// The popup view base.
    /// </summary>
    public class PopupViewBase : ViewBase, IPopupView
    {
        /// <inheritdoc />
        public event EventHandler<EventArgs> Closed;

        /// <inheritdoc />
        public void Close()
        {
            this.Close(EventArgs.Empty);
        }

        /// <inheritdoc />
        public void Close(EventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }

            var popup = this.Parent as Popup;
            Debug.Assert(popup != null, "popup != null");
            if (popup != null)
            {
                popup.IsOpen = false;
            }

            this.RaiseClosed(eventArgs);
        }

        private void RaiseClosed(EventArgs eventArgs)
        {
            var handler = this.Closed;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }
    }
}