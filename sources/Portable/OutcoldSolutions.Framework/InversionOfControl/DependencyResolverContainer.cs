// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Default implementation of <see cref="IDependencyResolverContainer"/>.
    /// </summary>
    public class DependencyResolverContainer : IDependencyResolverContainer
    {
        private readonly IDependencyResolverContainer parentContainer;
        private readonly string containerContext;
        private readonly Dictionary<string, DependencyResolverContainer> childrenContainers = new Dictionary<string, DependencyResolverContainer>();
        private readonly Dictionary<Type, ContainerInstance> registeredObjects = new Dictionary<Type, ContainerInstance>();
        private readonly object registractionContextLocker = new object();

        private readonly IContainerInstance currentInstanceContainer;

        private RegistrationContext currentRegistrationContext;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyResolverContainer"/> class.
        /// </summary>
        public DependencyResolverContainer()
        {
            this.Behavior = new DependencyResolverContainerBehavior();

            // Self register
            using (var registration = this.Registration())
            {
                (this.currentInstanceContainer = registration.Register(typeof(IDependencyResolverContainer)))
                    .And<DependencyResolverContainer>().AsSingleton(this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyResolverContainer"/> class. 
        /// The child container with specific <paramref name="containerContext"/>.
        /// </summary>
        /// <param name="parentContainer">
        /// The parent container.
        /// </param>
        /// <param name="containerContext">
        /// The context.
        /// </param>
        internal DependencyResolverContainer(IDependencyResolverContainer parentContainer, string containerContext)
            : this()
        {
            if (parentContainer == null)
            {
                throw new ArgumentNullException("parentContainer");
            }

            if (containerContext == null)
            {
                throw new ArgumentNullException("containerContext");
            }

            this.parentContainer = parentContainer;
            this.containerContext = containerContext;
        }

        ~DependencyResolverContainer()
        {
            this.Dispose(disposing: false);
        }

        internal event EventHandler Disposing;

        /// <summary>
        /// Gets the behavior of current dependency resolver container.
        /// </summary>
        public DependencyResolverContainerBehavior Behavior { get; private set; }

        /// <inheritdoc />
        public IRegistrationContext Registration()
        {
            this.CheckDisposed();

            lock (this.registractionContextLocker)
            {
                if (this.currentRegistrationContext == null)
                {
                    var registrationContext = new RegistrationContext(this);
                    registrationContext.Disposing += this.OnRegistrationContextDisposing;
                    return this.currentRegistrationContext = registrationContext;
                }

                throw new NotSupportedException(InversionOfControlResources.ErrMsg_PreviosCreatedContextIsNotDisposed);
            }
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
                DependencyResolverContainer container;
                if (!this.childrenContainers.TryGetValue(context, out container))
                {
                    container = this.childrenContainers[context] = new DependencyResolverContainer(this, context);
                    container.Disposing += this.OnChildContainerDisposing;
                }

                return container;
            }
        }

        /// <inheritdoc />
        public bool IsRegistered(Type type)
        {
            this.CheckDisposed();

            bool containsKey;

            lock (this.registractionContextLocker)
            {
                containsKey = this.registeredObjects.ContainsKey(type);
            }

            return containsKey
                    || (this.parentContainer != null && this.parentContainer.IsRegistered(type));
        }

        /// <inheritdoc />
        public bool IsRegistered<TType>()
        {
            return this.IsRegistered(typeof(TType));
        }

        /// <inheritdoc />
        public object Resolve(Type type, params object[] arguments)
        {
            ContainerInstance instance = this.Get(type);

            if (instance != null)
            {
                return instance.Resolve(arguments);
            }

            if (this.Behavior.AutoRegistration)
            {
                if (this.parentContainer == null || !this.parentContainer.IsRegistered(type))
                {
                    using (var registration = this.Registration())
                    {
                        instance = (ContainerInstance)registration.Register(type);
                    }

                    return instance.Resolve(arguments);
                }
            }

            if (this.parentContainer != null)
            {
                return this.parentContainer.Resolve(type, arguments);
            }

            throw new ArgumentOutOfRangeException(
                "type",
                string.Format(CultureInfo.CurrentCulture, InversionOfControlResources.ErrMsg_TypeIsNotRegistered, type));
        }

        /// <inheritdoc />
        public TType Resolve<TType>(params object[] arguments)
        {
            return (TType)this.Resolve(typeof(TType), arguments);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal void Add(Type type, ContainerInstance instance)
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

            lock (this.registractionContextLocker)
            {
                if (this.currentRegistrationContext == null)
                {
                    throw new NotSupportedException(
                        InversionOfControlResources.ErrMsg_ContainerIsNotBlockedForRegistration);
                }

                this.registeredObjects.Add(type, instance);
            }
        }

        internal ContainerInstance Get(Type type)
        {
            this.CheckDisposed();

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            bool locked = false;
            if (this.currentRegistrationContext != null)
            {
                Monitor.Enter(this.registractionContextLocker);
                locked = true;
            }

            try
            {
                ContainerInstance instance;
                return this.registeredObjects.TryGetValue(type, out instance) ? instance : null;
            }
            finally 
            {
                if (locked)
                {
                    Monitor.Exit(this.registractionContextLocker);
                }
            }
        }

        private void OnRegistrationContextDisposing(object sender, EventArgs eventArgs)
        {
            this.CheckDisposed();

            lock (this.registractionContextLocker)
            {
                if (this.currentRegistrationContext != sender)
                {
                    throw new ArgumentException("registrationContext");
                }

                this.currentRegistrationContext.Disposing -= this.OnRegistrationContextDisposing;
                this.currentRegistrationContext = null;
            }
        }

        private void OnChildContainerDisposing(object sender, EventArgs eventArgs)
        {
            this.CheckDisposed();

            lock (this.childrenContainers)
            {
                Debug.Assert(sender is DependencyResolverContainer, "sender is DependencyResolverContainer");

                var childContainer = sender as DependencyResolverContainer;
                if (childContainer != null)
                {
                    this.childrenContainers.Remove(childContainer.containerContext);
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.OnDisposing();

                    foreach (var containerInstance in this.registeredObjects.Values)
                    {
                        if (containerInstance != this.currentInstanceContainer)
                        {
                            containerInstance.Dispose();
                        }
                    }

                    this.registeredObjects.Clear();

                    foreach (var child in this.childrenContainers)
                    {
                        child.Value.Dispose();
                    }

                    this.childrenContainers.Clear();

                    if (this.currentRegistrationContext != null)
                    {
                        this.currentRegistrationContext.Disposing -= this.OnRegistrationContextDisposing;
                        this.currentRegistrationContext.Dispose();
                    }
                }

                this.isDisposed = true;
            }
        }

        private void OnDisposing()
        {
            EventHandler handler = this.Disposing;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void CheckDisposed()
        {
            if (this.isDisposed)
            {
                if (this.containerContext != null)
                {
                    var errMessage = string.Format(
                        CultureInfo.CurrentCulture,
                        InversionOfControlResources.ErrMsg_ContainerDisposedContext,
                        this.containerContext);

                    throw new ObjectDisposedException(
                        typeof(DependencyResolverContainer).Name,
                        errMessage);
                }
                else
                {
                    throw new ObjectDisposedException(
                        typeof(DependencyResolverContainer).Name,
                        InversionOfControlResources.ErrMsg_ContainerDisposed);
                }
            }
        }
    }
}