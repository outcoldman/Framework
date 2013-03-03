// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Suites.Web
{
    using System.Net.Http;

    using NUnit.Framework;

    using OutcoldSolutions.Web;

    public class HttpContentExtensionsSuites
    {
        [Test]
        public async void ReadAsDictionaryAsync_SimpleContent_AllValuesRead()
        {
            // Arrange
            HttpContent content = new StringContent(
@"SID=DQAAAGgA7Zg8CTN
LSID=DQAAAGsAlk8BBbG
Auth=DQAAAGgAdk3fA5N");

            // Act
            var dictionary = await content.ReadAsDictionaryAsync();

            // Assert
            Assert.AreEqual("DQAAAGgA7Zg8CTN", dictionary["SID"]);
            Assert.AreEqual("DQAAAGsAlk8BBbG", dictionary["LSID"]);
            Assert.AreEqual("DQAAAGgAdk3fA5N", dictionary["Auth"]);
        }

        [Test]
        public void IsPlainText_NullHeader_False()
        {
            // Arrange
            HttpContent content = new ByteArrayContent(new byte[] { });

            // Act
            bool result = content.IsPlainText();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsPlainText_PlainTextHeader_True()
        {
            // Arrange
            HttpContent content = new StringContent(string.Empty);

            // Act
            bool result = content.IsPlainText();

            // Assert
            Assert.IsTrue(result);
        }
    }
}