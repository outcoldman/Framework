// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Suites.Converters
{
    using System;

    using NUnit.Framework;

    using OutcoldSolutions.Converters;

    using Windows.UI.Xaml;

    public class ValueToVisibilityConverterSuites : PresentationSuitesBase
    {
        private const string EnglishLanguage = "en";
        private static readonly Type VisibilityType = typeof(Visibility);

        private ValueToVisibilityConverter converter;

        public override void SetUp()
        {
            base.SetUp();

            this.converter = new ValueToVisibilityConverter();
        }

        [Test]
        public void Convert_NotNull_Collapsed()
        {
            // Act
            var value = this.converter.Convert(new object(), VisibilityType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Collapsed, value);
        }

        [Test]
        public void Convert_Null_Visible()
        {
            // Act
            var value = this.converter.Convert(null, VisibilityType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Visible, value);
        }

        [Test]
        public void Convert_NotNullInverted_Visible()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.Convert(new object(), VisibilityType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Visible, value);
        }

        [Test]
        public void Convert_NullInverted_Collapsed()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.Convert(null, VisibilityType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Collapsed, value);
        }

        [Test]
        public void Convert_DoublesNotEqual_Collapsed()
        {
            // Act
            var value = this.converter.Convert(2.0d, VisibilityType, "1", EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Collapsed, value);
        }

        [Test]
        public void Convert_DoublesAreEqual_Visible()
        {
            // Act
            var value = this.converter.Convert(2.0d, VisibilityType, "2", EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Visible, value);
        }

        [Test]
        public void Convert_DoublesNotEqualInverted_Visible()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.Convert(2.0d, VisibilityType, "1", EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Visible, value);
        }

        [Test]
        public void Convert_DoublesAreEqualInverted_Collapsed()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.Convert(2.0d, VisibilityType, "2", EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Collapsed, value);
        }

        [Test]
        public void Convert_DoublesNotEqualCultureTest_Collapsed()
        {
            using (new CultureScope("ru"))
            {
                // Act
                var value = this.converter.Convert(2.0d, VisibilityType, "1.0", EnglishLanguage);


                // Assert
                Assert.AreEqual(Visibility.Collapsed, value);
            }
        }

        [Test]
        public void Convert_DoublesAreEqualCultureTest_Visible()
        {
            using (new CultureScope("ru"))
            {
                // Act
                var value = this.converter.Convert(2.0d, VisibilityType, "2.0", EnglishLanguage);

                // Assert
                Assert.AreEqual(Visibility.Visible, value);
            }
        }

        [Test]
        public void Convert_DoublesNotEqualInvertedCultureTest_Visible()
        {
            using (new CultureScope("ru"))
            {
                // Arrange
                this.converter.Invert = true;

                // Act
                var value = this.converter.Convert(2.0d, VisibilityType, "1.0", EnglishLanguage);

                // Assert
                Assert.AreEqual(Visibility.Visible, value);
            }
        }

        [Test]
        public void Convert_DoublesAreEqualInvertedCultureTest_Collapsed()
        {
            using (new CultureScope("ru"))
            {
                // Arrange
                this.converter.Invert = true;

                // Act
                var value = this.converter.Convert(2.0d, VisibilityType, "2.0", EnglishLanguage);

                // Assert
                Assert.AreEqual(Visibility.Collapsed, value);
            }
        }

        [Test]
        public void Convert_UInt16ValueAndTheSameStringParameter_Visible()
        {
            // Act
            var value = this.converter.Convert((ushort)1, VisibilityType, "1", EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Visible, value);
        }

        [Test]
        public void Convert_EnumToVisibilityConverterSameValue_Visible()
        {
            // Act
            var value = this.converter.Convert(FocusState.Keyboard, VisibilityType, (int)FocusState.Keyboard, EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Visible, value);
        }

        [Test]
        public void Convert_EnumToVisibilityConverterDifferentValue_Collapsed()
        {
            // Act
            var value = this.converter.Convert(FocusState.Keyboard, VisibilityType, (int)FocusState.Pointer, EnglishLanguage);

            // Assert
            Assert.AreEqual(Visibility.Collapsed, value);
        } 
    }
}