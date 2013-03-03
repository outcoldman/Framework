// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presenters
{
    using OutcoldSolutions.Shell;
    using OutcoldSolutions.Views;

    internal class ApplicationSettingFramePresenter : ViewPresenterBase<IApplicationSettingFrame>
    {
        private readonly IApplicationSettingViewsService applicationSettingViewsService;

        private object content;
        private string title;

        public ApplicationSettingFramePresenter(
            IDependencyResolverContainer container,
            IApplicationSettingViewsService applicationSettingViewsService)
            : base(container)
        {
            this.applicationSettingViewsService = applicationSettingViewsService;
            this.GoBackCommand = new DelegateCommand(this.GoBack);
        }

        public DelegateCommand GoBackCommand { get; set; }

        public string Title
        {
            get
            {
                return this.title;
            }

            private set
            {
                this.SetValue(ref this.title, value);
            }
        }

        public object Content
        {
            get
            {
                return this.content;
            }

            private set
            {
                this.SetValue(ref this.content, value);
            }
        }

        public void SetContent(string viewTitle, object viewContent)
        {
            this.Title = viewTitle;
            this.Content = viewContent;
        }

        private void GoBack()
        {
            this.View.Close();

            this.applicationSettingViewsService.Show();
        }
    }
}