// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Converters
{
    using System;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// The visibility converter base.
    /// </summary>
    public abstract class VisibilityConverterBase : IValueConverter 
    {
        /// <summary>
        /// Gets or sets a value indicating whether the value should be inverted.
        /// By default (<value>false</value> value) it converts <value>true</value> to <value>System.Visible</value> 
        /// and <value>false</value> to <value>System.Collapsed</value>. You can set <value>false</value> to change this.
        /// </summary>
        public bool Invert { get; set; }

        /// <inheritdoc />
        public abstract object Convert(object value, Type targetType, object parameter, string language);

        /// <inheritdoc />
        public abstract object ConvertBack(object value, Type targetType, object parameter, string language);

        protected Visibility ConvertToVisibility(bool result)
        {
            if (this.Invert)
            {
                result = !result;
            }

            if (result)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        protected bool ConvertToBoolean(Visibility visibility)
        {
            bool result = visibility == Visibility.Visible;
            
            if (this.Invert)
            {
                return !result;
            }
            else
            {
                return result;
            }
        }
    }
}