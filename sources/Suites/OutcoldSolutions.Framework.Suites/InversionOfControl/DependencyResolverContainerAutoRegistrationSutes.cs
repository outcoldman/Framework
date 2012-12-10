// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Framework.InversionOfControl
{
    using NUnit.Framework;

    public class DependencyResolverContainerAutoRegistrationSutes
    {
        [Test]
        public void Resolve_NotRegisteredType_ShouldBeResolved()
        {
            // Arrange
            var container = new DependencyResolverContainer
                                {
                                    Behavior = { AutoRegistration = true }
                                };

            // Act
            var instance = container.Resolve<ServiceStub>();

            // Assert
            Assert.IsNotNull(instance);
        }
    }
}
