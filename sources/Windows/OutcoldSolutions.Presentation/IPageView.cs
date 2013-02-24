// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    /// <summary>
    /// The PageView interface.
    /// </summary>
    public interface IPageView : IView
    {
        /// <summary>
        /// On navigated to.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        void OnNavigatedTo(NavigatedToEventArgs eventArgs);

        /// <summary>
        /// On navigating from.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        void OnNavigatingFrom(NavigatingFromEventArgs eventArgs); 
    }
}