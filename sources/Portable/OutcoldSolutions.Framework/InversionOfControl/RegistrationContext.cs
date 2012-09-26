// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    internal class RegistrationContext : IRegistrationContext
    {
        private readonly IDependencyResolverContainerEx container;

        public RegistrationContext(IDependencyResolverContainerEx container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

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

        public IContainerObjectInfo Register(Type type)
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(typeof(IRegistrationContext).Name);
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return new ContainerObjectInfo(type, this.container, this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this.container.RemoveRegistrationContext(this);
                }

                this.IsDisposed = true;
            }
        }
    }
}