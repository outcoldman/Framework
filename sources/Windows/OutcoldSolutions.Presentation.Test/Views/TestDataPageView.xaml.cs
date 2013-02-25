// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Presentation.Test.Views
{
    using OutcoldSolutions.Views;

    public interface ITestDataPageView : IDataPageView
    {
    }

    public sealed partial class TestDataPageView : DataPageViewBase, ITestDataPageView
    {
        public TestDataPageView()
        {
            this.InitializeComponent();
        }
    }
}
