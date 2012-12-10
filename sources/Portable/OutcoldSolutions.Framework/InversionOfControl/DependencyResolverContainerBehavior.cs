// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    /// <summary>
    /// The dependency resolver container behavior.
    /// </summary>
    public class DependencyResolverContainerBehavior
    {
        internal DependencyResolverContainerBehavior()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether auto registration should be on or off.
        /// </summary>
        /// <remarks>
        /// This value changes behavior of <see cref="DependencyResolverContainer.Resolve"/> method.
        /// If value is <value>true</value> and container will not find this type in registered types - 
        /// it will try to register it first before resolving (only in case if this is not abstract class).
        /// Default value is <value>false</value>
        /// </remarks>
        public bool AutoRegistration { get; set; }
    }
}
