// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Test.Views
{
    using OutcoldSolutions.Views;

    public interface IStartPageView : IPageView
    {
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPageView : PageViewBase, IStartPageView
    {
        public StartPageView()
        {
            this.InitializeComponent();
        }
    }
}
