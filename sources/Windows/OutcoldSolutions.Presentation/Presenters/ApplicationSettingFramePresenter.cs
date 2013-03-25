// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presenters
{
    using System;

    using OutcoldSolutions.BindingModels;
    using OutcoldSolutions.Shell;
    using OutcoldSolutions.Views;

    internal class ApplicationSettingFramePresenter : ViewPresenterBase<IApplicationSettingFrame>, IDisposable
    {
        private readonly IApplicationSettingViewsService applicationSettingViewsService;

        private object content;
        private string title;

        public ApplicationSettingFramePresenter(
            IApplicationSettingViewsService applicationSettingViewsService)
        {
            this.applicationSettingViewsService = applicationSettingViewsService;
            this.GoBackCommand = new DelegateCommand(this.GoBack);
        }

        ~ApplicationSettingFramePresenter()
        {
            this.Dispose(disposing: false);
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

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void GoBack()
        {
            this.View.Close();

            this.applicationSettingViewsService.Show();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Content.DisposeIfDisposable();
                var viewBase = this.Content as ViewBase;
                if (viewBase != null)
                {
                    viewBase.GetPresenter<BindingModelBase>().DisposeIfDisposable();
                }
            }
        }
    }
}