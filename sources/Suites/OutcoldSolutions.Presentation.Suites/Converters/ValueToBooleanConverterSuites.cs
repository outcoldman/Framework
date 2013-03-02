// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Suites.Converters
{
    using System;

    using NUnit.Framework;

    using OutcoldSolutions.Converters;

    using Windows.UI.Xaml;

    public class ValueToBooleanConverterSuites : PresentationSuitesBase
    {
        private const string EnglishLanguage = "en";
        private static readonly Type BooleanType = typeof(bool);

        private ValueToBooleanConverter converter;

        public override void SetUp()
        {
            base.SetUp();

            this.converter = new ValueToBooleanConverter();
        }

        [Test]
        public void Convert_NotNull_False()
        {
            // Act
            var value = this.converter.Convert(new object(), BooleanType, null, EnglishLanguage);

            // Assert
            Assert.IsFalse((bool)value);
        }

        [Test]
        public void Convert_Null_True()
        {
            // Act
            var value = this.converter.Convert(null, BooleanType, null, EnglishLanguage);

            // Assert
            Assert.IsTrue((bool)value);
        }

        [Test]
        public void Convert_NotNullInverted_False()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.Convert(new object(), BooleanType, null, EnglishLanguage);

            // Assert
            Assert.IsFalse((bool)value);
        }

        [Test]
        public void Convert_NullInverted_True()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.Convert(null, BooleanType, null, EnglishLanguage);

            // Assert
            Assert.IsTrue((bool)value);
        }

        [Test]
        public void Convert_DoublesNotEqual_False()
        {
            // Act
            var value = this.converter.Convert(2.0d, BooleanType, "1", EnglishLanguage);

            // Assert
            Assert.IsFalse((bool)value);
        }

        [Test]
        public void Convert_DoublesAreEqual_True()
        {
            // Act
            var value = this.converter.Convert(2.0d, BooleanType, "2", EnglishLanguage);

            // Assert
            Assert.IsTrue((bool)value);
        }

        [Test]
        public void Convert_DoublesNotEqualInverted_False()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.Convert(2.0d, BooleanType, "1", EnglishLanguage);

            // Assert
            Assert.IsFalse((bool)value);
        }

        [Test]
        public void Convert_DoublesAreEqualInverted_True()
        {
            // Arrange
            this.converter.Invert = true;

            // Act
            var value = this.converter.Convert(2.0d, BooleanType, "2", EnglishLanguage);

            // Assert
            Assert.IsTrue((bool)value);
        }

        [Test]
        public void Convert_DoublesNotEqualCultureTest_False()
        {
            using (new CultureScope("ru"))
            {
                // Act
                var value = this.converter.Convert(2.0d, BooleanType, "1.0", EnglishLanguage);


                // Assert
                Assert.IsFalse((bool)value);
            }
        }

        [Test]
        public void Convert_DoublesAreEqualCultureTest_True()
        {
            using (new CultureScope("ru"))
            {
                // Act
                var value = this.converter.Convert(2.0d, BooleanType, "2.0", EnglishLanguage);

                // Assert
                Assert.IsTrue((bool)value);
            }
        }

        [Test]
        public void Convert_DoublesNotEqualInvertedCultureTest_False()
        {
            using (new CultureScope("ru"))
            {
                // Arrange
                this.converter.Invert = true;

                // Act
                var value = this.converter.Convert(2.0d, BooleanType, "1.0", EnglishLanguage);

                // Assert
                Assert.IsFalse((bool)value);
            }
        }

        [Test]
        public void Convert_DoublesAreEqualInvertedCultureTest_True()
        {
            using (new CultureScope("ru"))
            {
                // Arrange
                this.converter.Invert = true;

                // Act
                var value = this.converter.Convert(2.0d, BooleanType, "2.0", EnglishLanguage);

                // Assert
                Assert.IsTrue((bool)value);
            }
        }

        [Test]
        public void Convert_UInt16ValueAndTheSameStringParameter_True()
        {
            // Act
            var value = this.converter.Convert((ushort)1, BooleanType, "1", EnglishLanguage);

            // Assert
            Assert.IsTrue((bool)value);
        }

        [Test]
        public void Convert_EnumToVisibilityConverterSameValue_True()
        {
            // Act
            var value = this.converter.Convert(FocusState.Keyboard, BooleanType, (int)FocusState.Keyboard, EnglishLanguage);

            // Assert
            Assert.IsTrue((bool)value);
        }

        [Test]
        public void Convert_EnumToVisibilityConverterDifferentValue_False()
        {
            // Act
            var value = this.converter.Convert(FocusState.Keyboard, BooleanType, (int)FocusState.Pointer, EnglishLanguage);

            // Assert
            Assert.IsFalse((bool)value);
        } 
    }
}