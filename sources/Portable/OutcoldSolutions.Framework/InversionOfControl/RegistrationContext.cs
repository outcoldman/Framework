// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    internal class RegistrationContext : IRegistrationContext
    {
        private readonly DependencyResolverContainer container;

        private bool isDisposed;

        public RegistrationContext(
            DependencyResolverContainer container)
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

        public event EventHandler Disposing;

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IContainerInstance Register(Type type)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(typeof(IRegistrationContext).Name);
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return new ContainerInstance(type, this.container);
        }

        public IContainerInstance Register<TType>()
        {
            return this.Register(typeof(TType));
        }

        private void OnDisposing()
        {
            EventHandler handler = this.Disposing;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.OnDisposing();
                }

                this.isDisposed = true;
            }
        }
    }
}