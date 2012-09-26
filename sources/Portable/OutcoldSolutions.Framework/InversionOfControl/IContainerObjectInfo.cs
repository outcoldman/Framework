// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The RegisteredObject interface.
    /// </summary>
    public interface IContainerObjectInfo
    {
        /// <summary>
        /// The implementation.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="IContainerObjectInfo"/>.
        /// </returns>
        IContainerObjectInfo And(Type type);

        /// <summary>
        /// Set implementation type for registered types.
        /// </summary>
        /// <param name="typeImplementation">
        /// The type implementation. Cannot be interface or abstracted class.
        /// </param>
        void As(Type typeImplementation);

        /// <summary>
        /// Set factory which can constructs registered types.
        /// </summary>
        /// <param name="factoryFunction">
        /// The function which can construct registered types.
        /// </param>
        void As(Func<object[], object> factoryFunction);

        /// <summary>
        /// Set implementation for registered types and mark it as singleton.
        /// </summary>
        /// <param name="typeImplementation">
        /// The type Implementation.
        /// </param>
        void AsSingleton(Type typeImplementation);

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
        void AsSingleton(Func<object[], object> factoryFunction);
    }
}