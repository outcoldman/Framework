// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Controls
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls.Primitives;

    /// <summary>
    /// The app bar toggle button.
    /// </summary>
    /// <remarks>
    /// This class just fixes the Visual State "Checked" / "Unchecked" for layout update.
    /// </remarks>
    public class AppBarToggleButton : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppBarToggleButton"/> class.
        /// </summary>
        public AppBarToggleButton()
        {
            this.Click += (sender, args) => this.UpdateState();
            this.LayoutUpdated += (sender, o) => this.UpdateState();
        }

        private void UpdateState()
        {
            VisualStateManager.GoToState(
                this, this.IsChecked.HasValue && this.IsChecked.Value ? "Checked" : "Unchecked", false);
        }
    }
}