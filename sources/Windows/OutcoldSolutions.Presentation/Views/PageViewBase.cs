// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    using OutcoldSolutions.Presenters;

    using Windows.UI.Xaml;

    /// <summary>
    /// The page view base.
    /// </summary>
    public class PageViewBase : ViewBase, IPageView
    {
        /// <summary>
        /// The title dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = 
            DependencyProperty.Register("Title", typeof(string), typeof(PageViewBase), new PropertyMetadata(null));

        /// <summary>
        /// The subtitle dependency property.
        /// </summary>
        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register("Subtitle", typeof(string), typeof(PageViewBase), new PropertyMetadata(null));

        /// <summary>
        /// The is title visible dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTitleVisibleProperty =
            DependencyProperty.Register("IsTitleVisible", typeof(bool), typeof(PageViewBase), new PropertyMetadata(true));

        /// <summary>
        /// The is store logo visible dependency property.
        /// </summary>
        public static readonly DependencyProperty IsStoreLogoVisibleProperty =
            DependencyProperty.Register("IsStoreLogoVisible", typeof(bool), typeof(PageViewBase), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        public string Subtitle
        {
            get { return (string)this.GetValue(SubtitleProperty); }
            set { this.SetValue(SubtitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is title visible.
        /// </summary>
        public bool IsTitleVisible
        {
            get { return (bool)this.GetValue(IsTitleVisibleProperty); }
            set { this.SetValue(IsTitleVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is store logo visible.
        /// </summary>
        public bool IsStoreLogoVisible
        {
            get { return (bool)this.GetValue(IsStoreLogoVisibleProperty); }
            set { this.SetValue(IsStoreLogoVisibleProperty, value); }
        }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        protected INavigationService NavigationService { get; private set; }

        /// <inheritdoc />
        public virtual void OnNavigatedTo(NavigatedToEventArgs eventArgs)
        {
            ((IPagePresenterBase)this.DataContext).OnNavigatedTo(eventArgs);
        }

        /// <inheritdoc />
        public virtual void OnNavigatingFrom(NavigatingFromEventArgs eventArgs)
        {
            ((IPagePresenterBase)this.DataContext).OnNavigatingFrom(eventArgs);
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.NavigationService = this.Container.Resolve<INavigationService>();
        }
    }
}
