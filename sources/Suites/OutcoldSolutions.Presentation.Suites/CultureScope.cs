// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Suites
{
    using System;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// This class set the current culture and current UI culture for current thread and restore original on dispose.
    /// </summary>
    public class CultureScope : IDisposable
    {
        private readonly CultureInfo culture;
        private readonly CultureInfo uiCulture;

        public CultureScope(string language)
            : this(new CultureInfo(language))
        {
        }

        public CultureScope(CultureInfo cultureInfo)
        {
            this.culture = Thread.CurrentThread.CurrentCulture;
            this.uiCulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        ~CultureScope()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = this.culture;
            Thread.CurrentThread.CurrentUICulture = this.uiCulture;

            GC.SuppressFinalize(this);
        }
    }
}