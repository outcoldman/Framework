// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Test.Presenters
{
    using System.Threading.Tasks;

    using OutcoldSolutions.Presentation.Test.BindingModel;
    using OutcoldSolutions.Presentation.Test.Views;
    using OutcoldSolutions.Presenters;

    public class TestDataPageViewPresenter : DataPagePresenterBase<ITestDataPageView, TestDataPageViewBindingModel>
    {
        public TestDataPageViewPresenter(IDependencyResolverContainer container)
            : base(container)
        {
        }

        protected override async Task LoadDataAsync(NavigatedToEventArgs navigatedToEventArgs)
        {
            await Task.Delay(1000);
        }
    }
}