// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    using System.Collections.Generic;

    /// <summary>
    /// The ApplicationToolbar interface.
    /// </summary>
    public interface IApplicationToolbar
    {
        /// <summary>
        /// Set menu items.
        /// </summary>
        /// <param name="menuItems">
        /// The menu items.
        /// </param>
        void SetMenuItems(IEnumerable<MenuItemMetadata> menuItems);

        /// <summary>
        /// Set view commands.
        /// </summary>
        /// <param name="commands">
        /// The commands.
        /// </param>
        void SetViewCommands(IEnumerable<CommandMetadata> commands);

        /// <summary>
        /// The clear view commands.
        /// </summary>
        void ClearViewCommands();

        /// <summary>
        /// Set context commands.
        /// </summary>
        /// <param name="commands">
        /// The commands.
        /// </param>
        void SetContextCommands(IEnumerable<CommandMetadata> commands);

        /// <summary>
        /// Clear context commands.
        /// </summary>
        void ClearContextCommands();

        /// <summary>
        /// Show popup.
        /// </summary>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <typeparam name="TPopup">
        /// The type of popup view.
        /// </typeparam>
        void ShowPopup<TPopup>(params object[] arguments) where TPopup : IPopupView;
    }
}