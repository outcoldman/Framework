// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    internal interface IContainerStore
    {
        void OnChildContainerDisposing(string context, IDependencyResolverContainer container);
    }
}