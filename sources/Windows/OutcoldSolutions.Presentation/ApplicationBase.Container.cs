// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using Windows.UI.Xaml;

    /// <summary>
    /// The application base static properties.
    /// </summary>
    public abstract partial class ApplicationBase : Application
    {
        /// <summary>
        /// Gets the container.
        /// </summary>
        public static IDependencyResolverContainer Container { get; internal set; }
    }
}