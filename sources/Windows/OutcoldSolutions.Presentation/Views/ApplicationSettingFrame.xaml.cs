// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    using OutcoldSolutions.Presenters;

    internal interface IApplicationSettingFrame : IPopupView
    {
        void SetContent(string title, object content);
    }

    internal sealed partial class ApplicationSettingFrame : PopupViewBase, IApplicationSettingFrame
    {
        private ApplicationSettingFramePresenter presenter;

        public ApplicationSettingFrame()
        {
            this.InitializeComponent();
        }

        public void SetContent(string title, object content)
        {
            this.presenter.SetContent(title, content);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.presenter = this.GetPresenter<ApplicationSettingFramePresenter>();
        }
    }
}
