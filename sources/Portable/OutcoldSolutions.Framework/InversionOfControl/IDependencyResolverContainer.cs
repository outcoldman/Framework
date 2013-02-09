// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The dependency resolver container interface. Provides inversion of control practice.
    /// </summary>
    /// <example>
    /// <code>
    /// IDependencyResolverContainer container = new DependencyResolverContainer();
    /// using (var regisration = container.Registration())
    /// {
    ///     registration.Register(typeof(IInterfaceA)).As(typeof(ClassA));
    /// }
    /// var instance = (IInterfaceA)container.Resolve(typeof(IInterfaceA));
    /// </code>
    /// </example>
    public interface IDependencyResolverContainer : IDisposable
    {
        /// <summary>
        /// Get registration context which allows to register types and instances for current container. This operation
        /// blocks all resolve operation untill instance of <see cref="IRegistrationContext"/> will be disposed.
        /// </summary>
        /// <returns>
        /// The <see cref="IRegistrationContext"/>. Allows to register types and instances in container.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// If previos created instance of <see cref="IRegistrationContext"/> is not disposed yet.
        /// </exception>
        IRegistrationContext Registration();

        /// <summary>
        /// Gets the context based dependency resolver container (child container). 
        /// </summary>
        /// <param name="context">
        /// The context, unique identifier for child container. Cannot be null.
        /// </param>
        /// <returns>
        /// The instance of child dependency resolver container <see cref="IDependencyResolverContainer"/>.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// If <paramref name="context"/> is null.
        /// </exception>
        IDependencyResolverContainer Context(string context);

        /// <summary>
        /// Checks if <paramref name="type"/> is registered and can be resolved with current container.
        /// If current container doesn't have registered type it will check parent container for it.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// If current type <paramref name="type"/> is registered.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// If <paramref name="type"/> is null.
        /// </exception>
        bool IsRegistered(Type type);

        /// <summary>
        /// Checks if <typeparamref name="TType"/> is registered and can be resolved with current container.
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
        /// Resolve instace for a type <paramref name="type"/>. 
        /// </summary>
        /// <param name="type">The type for resolve.</param>
        /// <param name="arguments">The list of arguments for constructor of current object and all required object which should be resolved in a chain.</param>
        /// <returns>Resolved object.</returns>
        /// <exception cref="NotSupportedException">
        /// If container is blocked by <see cref="IRegistrationContext"/> created by <see cref="Registration"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="type"/> cannot be resolved.
        /// </exception>
        object Resolve(Type type, params object[] arguments);

        /// <summary>
        /// Resolve instace for a type <typeparamref name="TType"/>. 
        /// </summary>
        /// <typeparam name="TType">The type for resolve.</typeparam>
        /// <param name="arguments">The list of arguments for constructor of current object and all required object which should be resolved in a chain.</param>
        /// <returns>Instance of resolved object.</returns>
        /// <exception cref="NotSupportedException">
        /// If container is blocked by <see cref="IRegistrationContext"/> created by <see cref="Registration"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <typeparamref name="TType"/> cannot be resolved.
        /// </exception>
        TType Resolve<TType>(params object[] arguments);
    }
}