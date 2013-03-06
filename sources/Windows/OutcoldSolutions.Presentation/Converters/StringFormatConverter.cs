// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Converters
{
    using System;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// The string format converter. Allows to use parameter as a strint format value.
    /// </summary>
    public class StringFormatConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the format (can be used by default if don't want to user paramtere in Convert method).
        /// </summary>
        public string Format { get; set; }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null && this.Format == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return string.Format("{0:" + (this.Format ?? parameter) + "}", value);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}