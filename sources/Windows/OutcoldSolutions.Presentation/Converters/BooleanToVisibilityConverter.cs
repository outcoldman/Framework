// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Converters
{
    using System;
    using System.Diagnostics;

    using Windows.UI.Xaml;

    /// <summary>
    /// UI Converter between <see cref="System.Boolean"/> and <see cref="Visibility"/>.
    /// By default it converts <value>true</value> to <value>System.Visible</value> and 
    /// <value>false</value> to <value>System.Collapsed</value>, but you can change this behavior with 
    /// <see cref="VisibilityConverterBase.Invert"/> property.
    /// </summary>
    public class BooleanToVisibilityConverter : VisibilityConverterBase
    {
        /// <inheritdoc />
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            Debug.Assert(targetType == typeof(Visibility), "targetType == typeof(Visibility)");

            if (value == null || !(value is bool))
            {
                return DependencyProperty.UnsetValue;
            }

            bool result = System.Convert.ToBoolean(value);
            
            return this.ConvertToVisibility(result);
        }

        /// <inheritdoc />
        public override object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Debug.Assert(targetType == typeof(bool), "targetType == typeof(bool)");

            if (value == null || !(value is Visibility))
            {
                return DependencyProperty.UnsetValue;
            }

            return this.ConvertToBoolean((Visibility)value);
        }
    }
}