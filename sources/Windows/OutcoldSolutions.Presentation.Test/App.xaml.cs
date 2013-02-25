// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Test
{
    using OutcoldSolutions.Presentation.Test.Presenters;
    using OutcoldSolutions.Presentation.Test.Views;
    using OutcoldSolutions.Presenters;

    /// <summary>
    /// The application base.
    /// </summary>
    public partial class App : ApplicationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBase"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override void InitializeApplication()
        {
            using (var registration = Container.Registration())
            {
                registration.Register<IStartPageView>()
                    .InjectionRule<PresenterBase, StartPageViewPresenter>()
                    .AsSingleton<StartPageView>();

                registration.Register<StartPageViewPresenter>().AsSingleton();
            }
        }

        protected override void Activated()
        {
            Container.Resolve<INavigationService>().NavigateTo<IStartPageView>();
        }
    }
}