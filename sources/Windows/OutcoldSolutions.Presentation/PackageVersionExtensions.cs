// --------------------------------------------------------------------------------------------------------------------
// OutcoldSolutions (http://outcoldsolutions.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System.Globalization;

    using Windows.ApplicationModel;

    /// <summary>
    /// The package version extension methods.
    /// </summary>
    public static class PackageVersionExtensions
    {
        /// <summary>
        /// Convert <see cref="PackageVersion"/> into version string.
        /// </summary>
        /// <param name="packageVersion">
        /// The package version.
        /// </param>
        /// <returns>
        /// The version <see cref="string"/>.
        /// </returns>
        public static string ToVersionString(this PackageVersion packageVersion)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "v{0}.{1}.{2}.{3}",
                packageVersion.Major,
                packageVersion.Minor,
                packageVersion.Build,
                packageVersion.Revision);
        }
    }
}