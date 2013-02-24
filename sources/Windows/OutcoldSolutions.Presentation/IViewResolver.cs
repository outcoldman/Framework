// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The view resolver interface.
    /// </summary>
    public interface IViewResolver
    {
        /// <summary>
        /// The get view type.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        Type GetViewType(object parameter);
    }
}