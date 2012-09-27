// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Implementation of <see cref="IDependencyResolverContainer"/>.
    /// </summary>
    public class DependencyResolverContainer : IContainerInstanceStore, IDependencyResolverContainer, IContainerStore
    {
        private readonly IDependencyResolverContainer parentContainer;
        private readonly IContainerStore containerStore;
        private readonly string containerContext;
        private readonly Dictionary<string, IDependencyResolverContainer> childrenContainers = new Dictionary<string, IDependencyResolverContainer>();
        private readonly Dictionary<Type, ContainerInstance> registeredObjects = new Dictionary<Type, ContainerInstance>();
        private readonly object registractionContextLocker = new object();

        private IRegistrationContext currentRegistrationContext;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyResolverContainer"/> class.
        /// </summary>
        public DependencyResolverContainer()
        {
        }

        ~DependencyResolverContainer()
        {
            this.Dispose(disposing: false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyResolverContainer"/> class.
        /// </summary>
        /// <param name="parentContainer">
        /// The parent container.
        /// </param>
        /// <param name="containerStore">
        /// The container store.
        /// </param>
        /// <param name="containerContext">
        /// The context.
        /// </param>
        internal DependencyResolverContainer(IDependencyResolverContainer parentContainer, IContainerStore containerStore, string containerContext)
        {
            this.CheckDisposed();

            if (parentContainer == null)
            {
                throw new ArgumentNullException("parentContainer");
            }

            if (containerContext == null)
            {
                throw new ArgumentNullException("containerContext");
            }

            this.parentContainer = parentContainer;
            this.containerStore = containerStore;
            this.containerContext = containerContext;
        }

        /// <inheritdoc />
        public IRegistrationContext Registration()
        {
            this.CheckDisposed();

            bool monitorTaken;
            if ((monitorTaken = Monitor.TryEnter(this.registractionContextLocker)) && this.currentRegistrationContext == null)
            {
                return this.currentRegistrationContext = new RegistrationContext(this, this);
            }

            if (monitorTaken)
            {
                Monitor.Exit(this.registractionContextLocker);
            }

            throw new NotSupportedException(FrameworkResources.ErrMsg_PreviosCreatedContextIsNotDisposed);
        }

        /// <inheritdoc />
        public IDependencyResolverContainer Context(string context)
        {
            this.CheckDisposed();

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            lock (this.childrenContainers)
            {
                IDependencyResolverContainer container;
                if (!this.childrenContainers.TryGetValue(context, out container))
                {
                    container = this.childrenContainers[context] = new DependencyResolverContainer(this, this, context);
                }

                return container;
            }
        }

        /// <inheritdoc />
        public bool IsRegistered(Type type)
        {
            this.CheckDisposed();

            lock (this.registractionContextLocker)
            {
                return this.registeredObjects.ContainsKey(type);
            }
        }

        /// <inheritdoc />
        public bool IsRegistered<TType>()
        {
            this.CheckDisposed();

            return this.IsRegistered(typeof(TType));
        }

        /// <inheritdoc />
        public object Resolve(Type type, params object[] arguments)
        {
            this.CheckDisposed();

            lock (this.registractionContextLocker)
            {
                if (this.currentRegistrationContext != null)
                {
                    throw new NotSupportedException(FrameworkResources.ErrMsg_ContainerLocked);
                }

                ContainerInstance instance;
                if (this.registeredObjects.TryGetValue(type, out instance))
                {
                    return instance.Resolve(arguments);
                }

                if (this.parentContainer != null)
                {
                    return this.parentContainer.Resolve(type, arguments);
                }

                throw new ArgumentOutOfRangeException(
                    "type",
                    string.Format(CultureInfo.CurrentCulture, FrameworkResources.ErrMsg_TypeIsNotRegistered, type));
            }
        }

        /// <inheritdoc />
        public TType Resolve<TType>(params object[] arguments)
        {
            this.CheckDisposed();

            return (TType)this.Resolve(typeof(TType), arguments);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(disposing: true);
        }

        void IContainerInstanceStore.OnRegistrationContextDisposing(IRegistrationContext registrationContext)
        {
            this.CheckDisposed();

            if (registrationContext == null)
            {
                throw new ArgumentNullException("registrationContext");
            }

            if (this.currentRegistrationContext != registrationContext)
            {
                throw new ArgumentException("registrationContext");
            }

            this.currentRegistrationContext = null;
            Monitor.Exit(this.registractionContextLocker);
        }

        void IContainerInstanceStore.Add(Type type, ContainerInstance instance, IRegistrationContext registrationContext)
        {
            this.CheckDisposed();

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (this.currentRegistrationContext == null || this.currentRegistrationContext != registrationContext)
            {
                throw new NotSupportedException(FrameworkResources.ErrMsg_ParentRegistrationContextIsDisposed);
            }

            this.registeredObjects.Add(type, instance);
        }

        ContainerInstance IContainerInstanceStore.Get(Type type)
        {
            this.CheckDisposed();

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ContainerInstance instance;
            return this.registeredObjects.TryGetValue(type, out instance) ? instance : null;
        }

        void IContainerStore.OnChildContainerDisposing(string context, IDependencyResolverContainer container)
        {
            this.CheckDisposed();

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            lock (this.childrenContainers)
            {
                IDependencyResolverContainer storedContainer;
                if (!this.childrenContainers.TryGetValue(context, out storedContainer))
                {
                    throw new IndexOutOfRangeException(string.Format(CultureInfo.CurrentCulture, FrameworkResources.ErrMsg_UnknownContext, context));
                }

                if (storedContainer != container)
                {
                    throw new ArgumentOutOfRangeException("container", FrameworkResources.ErrMsg_UnknownContainer);
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    if (this.containerStore != null)
                    {
                        this.containerStore.OnChildContainerDisposing(this.containerContext, this);
                    }

                    foreach (var containerInstance in this.registeredObjects.Values)
                    {
                        containerInstance.Dispose();
                    }

                    this.registeredObjects.Clear();

                    foreach (var child in this.childrenContainers)
                    {
                        child.Value.Dispose();
                    }

                    this.childrenContainers.Clear();
                }

                this.isDisposed = true;
            }
        }

        private void CheckDisposed()
        {
            if (this.isDisposed)
            {
                if (this.containerContext != null)
                {
                    throw new ObjectDisposedException(
                        typeof(DependencyResolverContainer).Name,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            FrameworkResources.ErrMsg_ContainerDisposedContext,
                            this.containerContext));
                }
                else
                {
                    throw new ObjectDisposedException(
                        typeof(DependencyResolverContainer).Name, 
                        FrameworkResources.ErrMsg_ContainerDisposed);
                }
            }
        }
    }
}