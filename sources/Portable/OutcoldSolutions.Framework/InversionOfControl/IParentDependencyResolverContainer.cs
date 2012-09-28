// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    /// <summary>
    /// Internal interface which provides additional methods for containers which can be parent containters (which can contains child containers).
    /// </summary>
    internal interface IParentDependencyResolverContainer : IDependencyResolverContainer
    {
        /// <summary>
        /// Notify parent container if child <paramref name="container"/> is disposing.
        /// </summary>
        /// <param name="context">The context with which child container was created.</param>
        /// <param name="container">The instance of the container.</param>
        void OnChildContainerDisposing(string context, IDependencyResolverContainer container);
    }
}