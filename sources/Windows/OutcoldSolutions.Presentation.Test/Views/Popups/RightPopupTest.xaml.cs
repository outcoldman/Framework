// --------------------------------------------------------------------------------------------------------------------
// OutcoldSolutions (http://outcoldsolutions.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Presentation.Test.Views.Popups
{
    using OutcoldSolutions.Views;

    public interface IRightPopupTest : IPopupView
    {
    }

    public sealed partial class RightPopupTest : PopupViewBase, IRightPopupTest
    {
        public RightPopupTest()
        {
            this.InitializeComponent();
        }
    }
}
