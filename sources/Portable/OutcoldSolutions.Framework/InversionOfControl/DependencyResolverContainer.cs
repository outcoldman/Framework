// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// The dependency resolver container.
    /// </summary>
    public class DependencyResolverContainer : IDependencyResolverContainer
    {
        private readonly IDictionary<Type, IContainerObjectInfo> registeredObjects = new Dictionary<Type, IContainerObjectInfo>();

        private readonly object locker = new object();

        private RegistrationContext currentRegistrationContext;

        public IRegistrationContext GetRegistrationContext()
        {
            Monitor.Enter(this.locker);

            if (Monitor.TryEnter(this.locker) && this.currentRegistrationContext == null)
            {
                return this.currentRegistrationContext = new RegistrationContext(this);
            }
            else
            {
                throw new NotSupportedException("Previous registration context is not disposed yet.");
            }
        }

        public bool IsRegistered(Type type)
        {
            return this.registeredObjects.ContainsKey(type);
        }

        private class ContainerObjectInfo : IContainerObjectInfo
        {
            private readonly DependencyResolverContainer container;

            public ContainerObjectInfo(DependencyResolverContainer container)
            {
                this.container = container;
            }

            public bool IsSingleton { get; private set; }

            public object Instance { get; private set; }

            public IContainerObjectInfo For(Type type)
            {
                this.container.registeredObjects.Add(type, this);
                return this;
            }

            public void AsSingleton()
            {
                this.IsSingleton = true;
            }

            public void AsSingleton(object instance)
            {
                this.Instance = instance;
            }
        }

        internal class RegistrationContext : IRegistrationContext
        {
            private readonly DependencyResolverContainer dependencyResolverContainer;

            public RegistrationContext(DependencyResolverContainer dependencyResolverContainer)
            {
                this.dependencyResolverContainer = dependencyResolverContainer;
            }

            public void Dispose()
            {
                Monitor.Exit(this.dependencyResolverContainer.locker);
                this.dependencyResolverContainer.currentRegistrationContext = null;
            }

            public IContainerObjectInfo Register(Type typeImplementation)
            {
                var info = new ContainerObjectInfo(this.dependencyResolverContainer);
                this.dependencyResolverContainer.registeredObjects.Add(typeImplementation, new ContainerObjectInfo(this.dependencyResolverContainer));
                return info;
            }
        }
    }
}