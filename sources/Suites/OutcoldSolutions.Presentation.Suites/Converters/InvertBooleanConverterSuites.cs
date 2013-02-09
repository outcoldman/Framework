// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Suites.Converters
{
    using System;

    using NUnit.Framework;

    using OutcoldSolutions.Presentation.Converters;
    using OutcoldSolutions.Presentation.Suites;

    public class InvertBooleanConverterSuites : SuitesBase
    {
        private const string EnglishLanguage = "en";
        private static readonly Type BooleanType = typeof(bool);

        private InvertBooleanConverter converter;

        public override void SetUp()
        {
            base.SetUp();

            this.converter = new InvertBooleanConverter();
        }

        [Test]
        public void Convert_True_False()
        {
            // Act
            var value = this.converter.ConvertBack(true, BooleanType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(false, value);
        }

        [Test]
        public void Convert_False_True()
        {
            // Act
            var value = this.converter.ConvertBack(false, BooleanType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(true, value);
        }

        [Test]
        public void ConvertBack_True_False()
        {
            // Act
            var value = this.converter.ConvertBack(true, BooleanType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(false, value);
        }

        [Test]
        public void ConvertBack_False_True()
        {
            // Act
            var value = this.converter.ConvertBack(false, BooleanType, null, EnglishLanguage);

            // Assert
            Assert.AreEqual(true, value);
        }
    }
}