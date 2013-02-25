// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Test.Presenters
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OutcoldSolutions.Presentation.Test.BindingModel;
    using OutcoldSolutions.Presentation.Test.Views;
    using OutcoldSolutions.Presenters;

    public class TestDataPageViewPresenter : DataPagePresenterBase<ITestDataPageView, TestDataPageViewBindingModel>
    {
        public TestDataPageViewPresenter(IDependencyResolverContainer container)
            : base(container)
        {
            this.Command = new DelegateCommand(() =>
                {
                    
                });
        }

        public DelegateCommand Command { get; set; }

        protected override async Task LoadDataAsync(NavigatedToEventArgs navigatedToEventArgs)
        {
            await Task.Delay(1000);
        }

        protected override IEnumerable<CommandMetadata> GetViewCommands()
        {
            yield return new CommandMetadata("ListAppBarButtonStyle", "List", this.Command);
        }
    }
}