// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Views
{
    /// <summary>
    /// The View interface.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Gets presenter.
        /// </summary>
        /// <typeparam name="TPresenter">
        /// Presenter type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="TPresenter"/>.
        /// </returns>
        TPresenter GetPresenter<TPresenter>();
    }
}