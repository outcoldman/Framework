// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Test.Presenters
{
    using OutcoldSolutions.Presentation.Test.Views;
    using OutcoldSolutions.Presenters;

    public class StartPageViewPresenter : PagePresenterBase<IStartPageView>
    {
        private string title;

        public StartPageViewPresenter(IDependencyResolverContainer container)
            : base(container)
        {
            this.Title = "Start View";
            this.ChangeTitleCommand = new DelegateCommand(() =>
                {
                    this.Title += " ;)";
                });
        }

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.RaiseCurrentPropertyChanged();
            }
        }

        public DelegateCommand ChangeTitleCommand { get; set; }
    }
}