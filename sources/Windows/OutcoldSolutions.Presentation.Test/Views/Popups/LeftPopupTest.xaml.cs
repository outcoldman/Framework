// --------------------------------------------------------------------------------------------------------------------
// OutcoldSolutions (http://outcoldsolutions.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Test.Views.Popups
{
    using OutcoldSolutions.Views;

    public interface ILeftPopupTest : IPopupView
    {
    }

    public sealed partial class LeftPopupTest : PopupViewBase, ILeftPopupTest
    {
        public LeftPopupTest()
        {
            this.InitializeComponent();
        }
    }
}
