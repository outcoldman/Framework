// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    /// <summary>
    /// The IMainFrameRegionProvider interface.
    /// </summary>
    public interface IMainFrameRegionProvider
    {
        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <param name="content">
        /// The content.
        /// </param>
        void SetContent(MainFrameRegion region, object content);

        /// <summary>
        /// Set content as <typeparamref name="TView"/>.
        /// </summary>
        /// <typeparam name="TView">The type of view.</typeparam>
        /// <param name="region">Specific region</param>
        /// <param name="injections">Injection for type resolving.</param>
        void SetContent<TView>(MainFrameRegion region, params object[] injections);

        /// <summary>
        /// Set visibility for region.
        /// </summary>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <param name="isVisible">
        /// The is visible.
        /// </param>
        void SetVisibility(MainFrameRegion region, bool isVisible);
    }
}