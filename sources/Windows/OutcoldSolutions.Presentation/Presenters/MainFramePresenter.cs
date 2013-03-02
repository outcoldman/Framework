// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presenters
{
    using OutcoldSolutions.BindingModels;
    using OutcoldSolutions.Views;

    internal class MainFramePresenter : ViewPresenterBase<IMainFrame>
    {
        private readonly INavigationService navigationService;

        public MainFramePresenter(
            IDependencyResolverContainer container,
            INavigationService navigationService)
            : base(container)
        {
            this.BindingModel = new MainFrameBindingModel();
            this.navigationService = navigationService;
            this.GoBackCommand = new DelegateCommand(this.GoBack, this.CanGoBack);
            this.navigationService.NavigatedTo += this.NavigationServiceOnNavigatedTo;
        }

        public DelegateCommand GoBackCommand { get; set; }

        public MainFrameBindingModel BindingModel { get; set; }

        public void NavigateTo(MenuItemMetadata menuItemMetadata)
        {
            if (menuItemMetadata != null)
            {
                if (menuItemMetadata.PageType != null)
                {
                    this.navigationService.NavigateTo(menuItemMetadata.PageType, menuItemMetadata.Parameter);
                }
                else
                {
                    this.navigationService.ResolveAndNavigateTo(menuItemMetadata.PageResolverType, menuItemMetadata.Parameter);
                }
            }
        }

        private bool CanGoBack()
        {
            return this.navigationService.CanGoBack();
        }

        private void GoBack()
        {
            if (this.CanGoBack())
            {
                this.navigationService.GoBack();
            }
        }

        private void NavigationServiceOnNavigatedTo(object sender, NavigatedToEventArgs navigatedToEventArgs)
        {
            this.GoBackCommand.RaiseCanExecuteChanged();
            this.BindingModel.IsBackButtonVisible = this.CanGoBack();
        }
    }
}