// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    /// <summary>
    /// The PagePresenterBase interface.
    /// </summary>
    public interface IPagePresenterBase
    {
        /// <summary>
        /// On navigated to.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        void OnNavigatedTo(NavigatedToEventArgs parameter);

        /// <summary>
        /// On navigating from.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        void OnNavigatingFrom(NavigatingFromEventArgs eventArgs);
    }
}