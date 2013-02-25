// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Suites.Converters
{
    using System;

    using NUnit.Framework;

    using OutcoldSolutions.Converters;

    using Windows.UI.Xaml;

    public class BooleanToVisibilityConverterSuites : PresentationSuitesBase
    {
        private const string EnglishLanguage = "en";
        private static readonly Type BooleanType = typeof(bool);
        private static readonly Type VisibilityType = typeof(Visibility);

        private BooleanToVisibilityConverter converter;

        public override void SetUp()
        {
            base.SetUp();

            this.converter = new BooleanToVisibilityConverter();
        }

        [Test]
        public void Convert_True_Visible()
        {
            // Act
            var value = this.converter.Convert(true, VisibilityType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Visible, value);
        }

        [Test]
        public void Convert_False_Collapsed()
        {
            // Act
            var value = this.converter.Convert(false, VisibilityType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Collapsed, value);
        }

        [Test]
        public void Convert_TrueInverted_Visible()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.Convert(true, VisibilityType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Collapsed, value);
        }

        [Test]
        public void Convert_FalseInverted_Collapsed()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.Convert(false, VisibilityType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Visible, value);
        }

        [Test]
        public void ConvertBack_Visible_True()
        {
            // Act
            var value = this.converter.ConvertBack(Visibility.Visible, BooleanType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(true, value);
        }

        [Test]
        public void ConvertBack_Collapsed_False()
        {
            // Act
            var value = this.converter.ConvertBack(Visibility.Collapsed, BooleanType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(false, value);
        }

        [Test]
        public void ConvertBack_VisibleInverted_True()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.ConvertBack(Visibility.Visible, BooleanType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(false, value);
        }

        [Test]
        public void ConvertBack_CollapsedInverted_False()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.ConvertBack(Visibility.Collapsed, BooleanType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(true, value);
        }
    }
}