// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Shell
{
    using System.Collections.Generic;

    using OutcoldSolutions.Views;

    /// <summary>
    /// The application setting views service interface.
    /// </summary>
    public interface IApplicationSettingViewsService
    {
        /// <summary>
        /// Show settings pane.
        /// </summary>
        void Show();

        /// <summary>
        /// Close settings pane.
        /// </summary>
        void Close();

        /// <summary>
        /// Show settings pane with active settings view.
        /// </summary>
        /// <param name="name">
        /// The name of setting view.
        /// </param>
        void Show(string name);

        /// <summary>
        /// Get registered view names.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{string}"/> (ordered by how they will be showed in settings).
        /// </returns>
        IEnumerable<string> GetRegisteredViews();

        /// <summary>
        /// Register settings view
        /// </summary>
        /// <typeparam name="TApplicationSettingsContent">The application settings view content.</typeparam>
        /// <param name="name">The unique name of the application settings view.</param>
        /// <param name="title">The title of the application settings view.</param>
        /// <param name="layoutType">The view type.</param>
        /// <param name="insertAfterName">Set the name of the settings which you want to see before this one.</param>
        void RegisterSettings<TApplicationSettingsContent>(
            string name, 
            string title, 
            ApplicationSettingLayoutType layoutType = ApplicationSettingLayoutType.Standard,
            string insertAfterName = null)
            where TApplicationSettingsContent : IApplicationSettingsContent;

        /// <summary>
        /// Unregister settings.
        /// </summary>
        /// <param name="name">
        /// The name of setting view.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>. Return false if setting view with the <paramref name="name"/> is not registered.
        /// </returns>
        bool UnregisterSettings(string name);
    }
}