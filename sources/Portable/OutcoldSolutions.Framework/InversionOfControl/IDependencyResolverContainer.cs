// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The dependency resolver container.
    /// </summary>
    public interface IDependencyResolverContainer
    {
        /// <summary>
        /// Get registration context to register types and instances in container. This operation
        /// blocks all resolve operation untill <see cref="IRegistrationContext"/> will be disposed.
        /// </summary>
        /// <returns>
        /// The <see cref="IRegistrationContext"/>. Allows to register types and instances in container.
        /// </returns>
        IRegistrationContext GetRegistrationContext();

        /// <summary>
        /// Check if <paramref name="type"/> is registered and can be resolved with current container.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// Is current type <paramref name="type"/> is registered.
        /// </returns>
        bool IsRegistered(Type type);

        /// <summary>
        /// Resolve type.
        /// </summary>
        /// <param name="type">The type for resolve.</param>
        /// <param name="arguments">The list of arguments for constructor.</param>
        /// <returns>Resolved object.</returns>
        object Resolve(Type type, object[] arguments = null);
    }
}