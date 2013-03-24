// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    using System.Diagnostics;

    using Windows.UI.Xaml.Controls.Primitives;

    /// <summary>
    /// The popup view base.
    /// </summary>
    public class PopupViewBase : ViewBase, IPopupView
    {
        /// <inheritdoc />
        public void Close()
        {
            this.OnClose();

            var popup = this.Parent as Popup;
            Debug.Assert(popup != null, "popup != null");
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }

        /// <summary>
        /// Invoked when popup view going to be closed.
        /// </summary>
        protected virtual void OnClose()
        {
        }
    }
}