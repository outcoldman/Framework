// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Presentation.Test.Views
{
    using OutcoldSolutions.Views;

    public interface ITestDataPageView : IPageView
    {
    }

    public sealed partial class TestDataPageView : PageViewBase, ITestDataPageView
    {
        public TestDataPageView()
        {
            this.InitializeComponent();
        }
    }
}
