// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Test.Presenters
{
    using OutcoldSolutions.Presentation.Test.Views;
    using OutcoldSolutions.Presentation.Test.Views.Popups;
    using OutcoldSolutions.Presenters;
    using OutcoldSolutions.Views;

    public class StartPageViewPresenter : PagePresenterBase<IStartPageView>
    {
        private string title;

        public StartPageViewPresenter()
        {
            this.Title = "Start View";
            this.ChangeTitleCommand = new DelegateCommand(() =>
                {
                    this.Title += " ;)";
                });

            this.ShowPopupCommand = new DelegateCommand(() => this.MainFrame.ShowPopup<IPopupTest>(PopupRegion.Full));
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

        public DelegateCommand ShowPopupCommand { get; set; }
    }
}