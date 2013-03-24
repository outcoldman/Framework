// --------------------------------------------------------------------------------------------------------------------
// OutcoldSolutions (http://outcoldsolutions.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Test.Views.Popups
{
    using OutcoldSolutions.Views;

    using Windows.UI.Xaml;

    public interface IPopupTest : IPopupView
    {
    }

    public sealed partial class PopupTest : PopupViewBase, IPopupTest
    {
        public PopupTest()
        {
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
