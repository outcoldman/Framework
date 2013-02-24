// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;

    using OutcoldSolutions.Diagnostics;

    /// <summary>
    /// The navigation service.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly LinkedList<HistoryItem> viewsHistory = new LinkedList<HistoryItem>();

        private readonly ILogger logger;
        private readonly IDependencyResolverContainer container;

        private IViewRegionProvider viewRegionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        public NavigationService(
            IDependencyResolverContainer container,
            ILogManager logManager)
        {
            this.container = container;
            this.logger = logManager.CreateLogger("NavigationService");
        }

        /// <inheritdoc />
        public event EventHandler<NavigatedToEventArgs> NavigatedTo;

        /// <inheritdoc />
        public void RegisterRegionProvider(IViewRegionProvider regionProvider)
        {
            if (regionProvider == null)
            {
                throw new ArgumentNullException("regionProvider");
            }

            this.viewRegionProvider = regionProvider;
        }

        /// <inheritdoc />
        public IPageView NavigateToView<TViewResolver>(object parameter, bool keepInHistory = true) where TViewResolver : IViewResolver
        {
            var viewResolver = this.container.Resolve<TViewResolver>();
            var pageViewType = viewResolver.GetViewType(parameter);
            return this.NavigateTo(pageViewType, parameter, keepInHistory);
        }

        /// <inheritdoc />
        public TView NavigateTo<TView>(object parameter = null, bool keepInHistory = true) where TView : IPageView
        {
            return (TView)this.NavigateTo(typeof(TView), parameter, keepInHistory);
        }

        /// <inheritdoc />
        public void GoBack()
        {
            if (this.viewRegionProvider == null)
            {
                throw new NotSupportedException("Register region provider first.");
            }

            this.logger.Debug("Go back requested");

            if (this.CanGoBack())
            {
                this.viewsHistory.RemoveLast();
                var item = this.viewsHistory.Last.Value;

                this.viewRegionProvider.Show(item.View);
                var navigatedToEventArgs = new NavigatedToEventArgs(item.View, item.State, item.Parameter, isBack: true);
                item.View.OnNavigatedTo(navigatedToEventArgs);
                this.RaiseNavigatedTo(navigatedToEventArgs);
            }
        }

        /// <inheritdoc />
        public bool CanGoBack()
        {
            return this.viewsHistory.Count > 1;
        }

        /// <inheritdoc />
        public bool HasHistory()
        {
            return this.viewsHistory.Count > 0;
        }

        /// <inheritdoc />
        public void ClearHistory()
        {
            this.viewsHistory.Clear();
        }

        private IPageView NavigateTo(Type pageViewType, object parameter = null, bool keepInHistory = true)
        {
            if (this.viewRegionProvider == null)
            {
                throw new NotSupportedException("Register region provider first.");
            }

            this.logger.Debug("Navigating to {0}. Parameter {1}.", pageViewType, parameter);

            IView currentView = null;

            if (this.viewsHistory.Count > 0)
            {
                var value = this.viewsHistory.Last.Value;
                if (object.Equals(value.Parameter, parameter)
                    && value.ViewType == pageViewType)
                {
                    this.logger.Warning("Double click found. Ignoring...");
                    return value.View;
                }

                currentView = this.viewsHistory.Last.Value.View;

                this.viewsHistory.Last.Value.View.OnNavigatingFrom(new NavigatingFromEventArgs(this.viewsHistory.Last.Value.State));
            }

            var view = (IPageView)this.container.Resolve(pageViewType);

            HistoryItem historyItem = null;
            if (keepInHistory)
            {
                historyItem = new HistoryItem(view, pageViewType, parameter);
                this.viewsHistory.AddLast(historyItem);
            }

            if (currentView == null || !currentView.Equals(view))
            {
                this.viewRegionProvider.Show(view);
            }
            else
            {
                this.logger.Debug("View the same: {0}.", pageViewType);
            }

            var navigatedToEventArgs = new NavigatedToEventArgs(view, historyItem == null ? null : historyItem.State, parameter, isBack: false);
            view.OnNavigatedTo(navigatedToEventArgs);
            this.RaiseNavigatedTo(navigatedToEventArgs);

            return view;
        }

        private void RaiseNavigatedTo(NavigatedToEventArgs eventArgs)
        {
            var handler = this.NavigatedTo;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

        private class HistoryItem
        {
            public HistoryItem(IPageView view, Type viewType, object parameter)
            {
                this.View = view;
                this.ViewType = viewType;
                this.Parameter = parameter;
                this.State = new Dictionary<string, object>();
            }

            public IPageView View { get; private set; }

            public Type ViewType { get; private set; }

            public object Parameter { get; private set; }

            public IDictionary<string, object> State { get; private set; }
        }
    }
}