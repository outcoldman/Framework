// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
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