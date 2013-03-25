// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OutcoldSolutions.Diagnostics;
    using OutcoldSolutions.Views;

    using Windows.UI.ApplicationSettings;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;

    internal class ApplicationSettingViewsService : IApplicationSettingViewsService
    {
        private const double SettingsWidth = 346;
        private const double LargeSettingsWidth = 646;

        private readonly List<string> settingViewsOrder = new List<string>();
        private readonly Dictionary<string, ApplicationSettingViewInfo> settingViewInfos = new Dictionary<string, ApplicationSettingViewInfo>();
        private readonly List<Popup> activePopups = new List<Popup>();

        private readonly ILogger logger;
        private readonly IDependencyResolverContainer container;

        private bool isSubscribed = false;

        public ApplicationSettingViewsService(
            ILogManager logManager,
            IDependencyResolverContainer container)
        {
            this.logger = logManager.CreateLogger("ApplicationSettingViewsService");
            this.container = container;
        }

        public void Show()
        {
            SettingsPane.Show();
        }

        public void Close()
        {
            foreach (var popupView in this.activePopups)
            {
                popupView.IsOpen = false;
            }
        }

        public void Show(string name)
        {
            var applicationSettingViewInfo = this.settingViewInfos[name];
            if (applicationSettingViewInfo != null)
            {
                this.CreatePopup(applicationSettingViewInfo);
            }
        }

        public IEnumerable<string> GetRegisteredViews()
        {
            return this.settingViewsOrder.ToList();
        }

        public void RegisterSettings<TApplicationSettingsView>(
            string name, 
            string title, 
            ApplicationSettingLayoutType layoutType = ApplicationSettingLayoutType.Standard,
            string insertAfterName = null) 
            where TApplicationSettingsView : IApplicationSettingsContent
        {
            int newIndex = this.settingViewsOrder.Count;

            if (insertAfterName != null)
            {
                int insertAfterIndex = this.settingViewsOrder.IndexOf(insertAfterName);
                if (insertAfterIndex > 0)
                {
                    newIndex = insertAfterIndex + 1;
                }
            }

            this.settingViewsOrder.Insert(newIndex, name);
            this.settingViewInfos[name] = new ApplicationSettingViewInfo(title, layoutType, typeof(TApplicationSettingsView));
            this.VerifyCommandsRequestedSubscription();
        }

        public bool UnregisterSettings(string name)
        {
            var isRemoved = this.settingViewInfos.Remove(name)
                            && this.settingViewsOrder.Remove(name);
            this.VerifyCommandsRequestedSubscription();
            return isRemoved;
        }

        private void VerifyCommandsRequestedSubscription()
        {
            if (this.settingViewInfos.Count > 0)
            {
                if (!this.isSubscribed)
                {
                    SettingsPane.GetForCurrentView().CommandsRequested += this.CommandsRequested;
                    this.isSubscribed = true;
                }
            }
            else
            {
                if (this.isSubscribed)
                {
                    SettingsPane.GetForCurrentView().CommandsRequested -= this.CommandsRequested;
                    this.isSubscribed = false;
                }
            }
        }

        private void CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            foreach (var viewInfo in this.settingViewInfos)
            {
                ApplicationSettingViewInfo info = viewInfo.Value;
                var cmd = new SettingsCommand(viewInfo.Key, info.Title, (x) => this.CreatePopup(info));
                args.Request.ApplicationCommands.Add(cmd);
            }
        }

        private void CreatePopup(ApplicationSettingViewInfo viewInfo)
        {
            var applicationSettingFrame = this.container.Resolve<IApplicationSettingFrame>();

            var settingsWidth = viewInfo.LayoutType == ApplicationSettingLayoutType.Large
                                    ? LargeSettingsWidth
                                    : SettingsWidth;

            var settingsPopup = new Popup();
            settingsPopup.Closed += this.OnPopupClosed;
            Window.Current.Activated += this.OnWindowActivated;
            settingsPopup.IsLightDismissEnabled = true;
            settingsPopup.Width = settingsWidth;
            settingsPopup.Height = Window.Current.Bounds.Height;

            ((FrameworkElement)applicationSettingFrame).Height = settingsPopup.Height;
            ((FrameworkElement)applicationSettingFrame).Width = settingsPopup.Width;

            applicationSettingFrame.SetContent(viewInfo.Title, this.container.Resolve(viewInfo.ViewType));

            settingsPopup.Child = (UIElement)applicationSettingFrame;
            settingsPopup.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width - settingsWidth);
            settingsPopup.SetValue(Canvas.TopProperty, 0);
            settingsPopup.IsOpen = true;

            this.activePopups.Add(settingsPopup);
        }

        private void OnWindowActivated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                this.Close();
            }
        }

        private void OnPopupClosed(object sender, object e)
        {
            var popup = sender as Popup;
            if (popup != null)
            {
                Window.Current.Activated -= this.OnWindowActivated;
                popup.Closed -= this.OnPopupClosed;
                this.activePopups.Remove(popup);

                try
                {
                    popup.Child.DisposeIfDisposable();
                }
                catch (Exception exception)
                {
                    this.logger.Error("Exception while tried to dispose popup view content.");
                    this.logger.LogErrorException(exception);
                }
            }
        }

        private class ApplicationSettingViewInfo
        {
            public ApplicationSettingViewInfo(
                string title, 
                ApplicationSettingLayoutType layoutType, 
                Type viewType)
            {
                this.Title = title;
                this.LayoutType = layoutType;
                this.ViewType = viewType;
            }

            public string Title { get; private set; }

            public ApplicationSettingLayoutType LayoutType { get; private set; }

            public Type ViewType { get; private set; }
        }
    }
}