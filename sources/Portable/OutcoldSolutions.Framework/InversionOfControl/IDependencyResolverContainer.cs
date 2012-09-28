// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The dependency resolver container interface.
    /// </summary>
    public interface IDependencyResolverContainer : IDisposable
    {
        /// <summary>
        /// Get registration context to register types and instances in container. This operation
        /// blocks all resolve operation untill <see cref="IRegistrationContext"/> will be disposed.
        /// </summary>
        /// <returns>
        /// The <see cref="IRegistrationContext"/>. Allows to register types and instances in container.
        /// </returns>
        IRegistrationContext Registration();

        /// <summary>
        /// Gets the context based dependency resolver container (child container). 
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The instance of dependency resolver container <see cref="IDependencyResolverContainer"/>.
        /// </returns>
        IDependencyResolverContainer Context(string context);

        /// <summary>
        /// Check if <paramref name="type"/> is registered and can be resolved with current container.
        /// If current container doesn't have registered type it will check parent container for it.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// Is current type <paramref name="type"/> is registered.
        /// </returns>
        bool IsRegistered(Type type);

        /// <summary>
        /// Check if <typeparamref name="TType"/> is registered and can be resolved with current container.
        /// If current container doesn't have registered type it will check parent container for it.
        /// </summary>
        /// <typeparam name="TType">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// Is current type <typeparamref name="TType"/> is registered.
        /// </returns>
        bool IsRegistered<TType>();

        /// <summary>
        /// Resolve instace with a type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type for resolve.</param>
        /// <param name="arguments">The list of arguments for constructor.</param>
        /// <returns>Resolved object.</returns>
        object Resolve(Type type, params object[] arguments);

        /// <summary>
        /// Resolve type.
        /// </summary>
        /// <typeparam name="TType">The type for resolve.</typeparam>
        /// <param name="arguments">The list of arguments for constructor.</param>
        /// <returns>Resolved object.</returns>
        TType Resolve<TType>(params object[] arguments);
    }
}