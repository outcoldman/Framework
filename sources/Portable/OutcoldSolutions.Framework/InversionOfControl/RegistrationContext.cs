// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    internal class RegistrationContext : IRegistrationContext
    {
        private readonly IContainerInstanceStore store;
        private readonly IDependencyResolverContainer container;

        public RegistrationContext(IContainerInstanceStore store, IDependencyResolverContainer container)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.store = store;
            this.container = container;
        }

        ~RegistrationContext()
        {
            this.Dispose(disposing: false);
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IContainerInstance Register(Type type)
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(typeof(IRegistrationContext).Name);
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return new ContainerInstance(type, this.store, this, this.container);
        }

        private void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this.store.OnRegistrationContextDisposing(this);
                }

                this.IsDisposed = true;
            }
        }
    }
}