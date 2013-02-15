// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The RegisteredObject interface.
    /// </summary>
    public interface IContainerInstance
    {
        /// <summary>
        /// The implementation.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="IContainerInstance"/>.
        /// </returns>
        IContainerInstance And(Type type);

        /// <summary>
        /// The implementation.
        /// </summary>
        /// <typeparam name="TType">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IContainerInstance"/>.
        /// </returns>
        IContainerInstance And<TType>();

        /// <summary>
        /// Set rule for injecting type.
        /// </summary>
        /// <param name="type">
        /// Injecting type.
        /// </param>
        /// <param name="implementation">
        /// Injecting implementation.
        /// </param>
        /// <returns>
        /// The <see cref="IContainerInstance"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="type"/> or <paramref name="implementation"/> is null.</exception>
        /// <exception cref="ArgumentException">If <paramref name="implementation"/> is not a implementation of type <paramref name="type"/> or if it is interface or abstract class.</exception>
        /// <exception cref="ArgumentException">If current registration already has a rule for type <paramref name="type"/> .</exception>
        IContainerInstance InjectionRule(Type type, Type implementation);

        /// <summary>
        /// Set rule for injecting type.
        /// </summary>
        /// <typeparam name="TType">
        /// Injecting type.
        /// </typeparam>
        /// <typeparam name="TImplementation">
        /// Injecting implementation.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IContainerInstance"/>.
        /// </returns>
        IContainerInstance InjectionRule<TType, TImplementation>() where TImplementation : TType;

        /// <summary>
        /// Set implementation type for registered types.
        /// </summary>
        /// <param name="typeImplementation">
        /// The type implementation. Cannot be interface or abstracted class.
        /// </param>
        void As(Type typeImplementation);

        /// <summary>
        /// Set implementation type for registered types.
        /// </summary>
        /// <typeparam name="TType">
        /// The type implementation. Cannot be interface or abstracted class.
        /// </typeparam>
        void As<TType>();

        /// <summary>
        /// Set factory which can constructs registered types.
        /// </summary>
        /// <param name="factoryFunction">
        /// The function which can construct registered types.
        /// </param>
        void As(Func<object> factoryFunction);

        /// <summary>
        /// Set factory which can constructs registered types.
        /// </summary>
        /// <param name="factoryFunction">
        /// The function which can construct registered types.
        /// </param>
        void As(Func<object[], object> factoryFunction);

        /// <summary>
        /// Mark registered type as singleton.
        /// </summary>
        void AsSingleton();

        /// <summary>
        /// Set implementation for registered types and mark it as singleton.
        /// </summary>
        /// <param name="typeImplementation">
        /// The type Implementation.
        /// </param>
        void AsSingleton(Type typeImplementation);

        /// <summary>
        /// Set implementation for registered types and mark it as singleton.
        /// </summary>
        /// <typeparam name="TType">
        /// The type Implementation.
        /// </typeparam>
        void AsSingleton<TType>();

        /// <summary>
        /// Set singleton implementation for registered types.
        /// </summary>
        /// <param name="instanceObject">
        /// The instance.
        /// </param>
        void AsSingleton(object instanceObject);

        /// <summary>
        /// Set singleton implementation for registered types. Instances will be resolved just once with function <paramref name="factoryFunction"/>.
        /// </summary>
        /// <param name="factoryFunction">
        /// The function which can construct registered types.
        /// </param>
        void AsSingleton(Func<object> factoryFunction);

        /// <summary>
        /// Set singleton implementation for registered types. Instances will be resolved just once with function <paramref name="factoryFunction"/>.
        /// </summary>
        /// <param name="factoryFunction">
        /// The function which can construct registered types.
        /// </param>
        void AsSingleton(Func<object[], object> factoryFunction);
    }
}