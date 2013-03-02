// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Converters
{
    using System;
    using System.Globalization;

    using Windows.UI.Xaml;

    /// <summary>
    /// UI converter to compare value and parameter and return <see cref="Visibility.Visible"/> if they are equal or 
    /// or <see cref="Visibility.Collapsed"/> if not. <see cref="VisibilityConverterBase.Invert"/> can invert return values.
    /// </summary>
    /// <remarks>
    /// This converter can be used as a NullToVisibilityConverter when parameter is null (not set).
    /// </remarks>
    public class ValueToVisibilityConverter : VisibilityConverterBase 
    {
        /// <inheritdoc />
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            bool result;
            if (parameter == null)
            {
                result = value == null;
            }
            else if (value == null)
            {
                result = false;
            }
            else if (value is double)
            {
                var d = System.Convert.ToDouble(parameter, this.GetCultureInfo(language));
                result = Math.Abs(((double)value) - d) < 0.00001;
            }
            else if (value is Enum)
            {
                result = value.Equals(Enum.ToObject(value.GetType(), parameter));
            }
            else
            {
                object parameterValue = System.Convert.ChangeType(parameter, value.GetType(), this.GetCultureInfo(language));
                result = value.Equals(parameterValue);
            }

            return this.ConvertToVisibility(result);
        }

        /// <inheritdoc />
        public override object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private CultureInfo GetCultureInfo(string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                return CultureInfo.CurrentCulture;
            }
            
            return new CultureInfo(language);
        }
    }
}