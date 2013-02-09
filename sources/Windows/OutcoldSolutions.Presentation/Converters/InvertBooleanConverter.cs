// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Converters
{
    using System;
    using System.Diagnostics;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// UI Converter to invert <see cref="System.Boolean"/> value to opposite value.
    /// </summary>
    public class InvertBooleanConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            Debug.Assert(targetType == typeof(bool), "targetType == typeof(bool)");

            return this.ConvertValue(value);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            Debug.Assert(targetType == typeof(bool), "targetType == typeof(bool)");

            return this.ConvertValue(value);
        }

        private object ConvertValue(object value)
        {
            if (value == null || !(value is bool))
            {
                return DependencyProperty.UnsetValue;
            }

            return !(bool)value;
        }
    }
}