﻿// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Test.Presenters
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
            this.ShowLeftPopupCommand = new DelegateCommand(() => this.MainFrame.ShowPopup<ILeftPopupTest>(PopupRegion.AppToolBarLeft));
            this.ShowRightPopupCommand = new DelegateCommand(() => this.MainFrame.ShowPopup<IRightPopupTest>(PopupRegion.AppToolBarRight));
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

        public DelegateCommand ShowLeftPopupCommand { get; set; }

        public DelegateCommand ShowRightPopupCommand { get; set; }

        protected override Task LoadDataAsync(NavigatedToEventArgs navigatedToEventArgs)
        {
            return Task.FromResult<object>(null);
        }

        protected override IEnumerable<CommandMetadata> GetViewCommands()
        {
            yield return new CommandMetadata("MoreAppBarButtonStyle", "Left popup", this.ShowLeftPopupCommand);
            yield return new CommandMetadata("MoreAppBarButtonStyle", "Right popup", this.ShowRightPopupCommand);
        }
    }
}