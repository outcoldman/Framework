// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The dependency resolver container.
    /// </summary>
    public class DependencyResolverContainer : IDependencyResolverContainer
    {
        private readonly Container container = new Container();

        /// <inheritdoc />
        public IRegistrationContext GetRegistrationContext()
        {
            IRegistrationContext registrationContext;
            if (this.container.TryLockForRegistrationContext(out registrationContext))
            {
                return registrationContext;
            }
            else
            {
                throw new NotSupportedException("Previous registration context is not disposed yet.");
            }
        }

        /// <inheritdoc />
        public bool IsRegistered(Type type)
        {
            return this.container.IsRegistered(type);
        }
    }
}