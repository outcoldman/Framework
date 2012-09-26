// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;

    internal class ContainerObjectInfo : IContainerObjectInfo
    {
        private readonly IDependencyResolverContainerEx container;
        private readonly IRegistrationContext registrationContext;
        private readonly List<Type> registeredTypes = new List<Type>();

        private Type implementation;
        private Func<object> factory;
        private object instance;

        public ContainerObjectInfo(Type type, IDependencyResolverContainerEx container, IRegistrationContext registrationContext)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (registrationContext == null)
            {
                throw new ArgumentNullException("registrationContext");
            }

            this.container = container;
            this.registrationContext = registrationContext;

            this.And(type);
        }

        public bool IsSingleton { get; private set; }

        public IContainerObjectInfo And(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (this.registrationContext.IsDisposed)
            {
                throw new ObjectDisposedException(typeof(IRegistrationContext).Name);
            }

            this.container.Add(type, this, this.registrationContext);
            this.registeredTypes.Add(type);
            return this;
        }

        public void As(Type typeImplementation)
        {
            if (typeImplementation == null)
            {
                throw new ArgumentNullException("typeImplementation");
            }

            this.CheckIsCompleted();

            this.implementation = typeImplementation;
        }

        public void As(Func<object> factoryFunction)
        {
            if (factoryFunction == null)
            {
                throw new ArgumentNullException("factoryFunction");
            }

            this.CheckIsCompleted();

            this.factory = factoryFunction;
        }

        public void AsSingleton(Type typeImplementation)
        {
            if (typeImplementation == null)
            {
                throw new ArgumentNullException("typeImplementation");
            }

            this.CheckIsCompleted();

            this.implementation = typeImplementation;
            this.IsSingleton = true;
        }

        public void AsSingleton(object instanceObject)
        {
            if (instanceObject == null)
            {
                throw new ArgumentNullException("instanceObject");
            }

            this.CheckIsCompleted();

            this.instance = instanceObject;
        }

        private void CheckIsCompleted()
        {
            if (this.factory != null || this.implementation != null || this.instance != null)
            {
                throw new NotSupportedException(FrameworkResources.ErrMsg_CannotSetMoreThanOneBehavior);
            }
        }
    }
}