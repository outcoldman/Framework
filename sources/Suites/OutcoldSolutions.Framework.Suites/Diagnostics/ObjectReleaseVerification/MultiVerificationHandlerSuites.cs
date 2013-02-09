// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.Framework.Diagnostics.ObjectReleaseVerification
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using OutcoldSolutions.Diagnostics;

    public class MultiVerificationHandlerSuites
    {
        [Test]
        public void VerificationSucceeded_ListOfReleasedObjects_ShouldPassListToHandlers()
        {
            // Arrange
            var handler = new Mock<IVerificationHandler>();

            var trackingObject = new Mock<ITrackingObject>();
            var releasedObjects = new List<ITrackingObject>() { trackingObject.Object };

            var multiHandler = new MultiVerificationHandler(new List<IVerificationHandler>() { handler.Object });

            // Act
            multiHandler.VerificationSucceeded(releasedObjects);

            // Assert
            handler.Verify(
                (h) =>
                h.VerificationSucceeded(
                    It.Is<IEnumerable<ITrackingObject>>((value) =>
                                                        !object.ReferenceEquals(value, releasedObjects)
                                                        && value.First() == trackingObject.Object)),
                Times.Once());
        }

        [Test]
        public void VerificaionFailed_ListOfReleasedAndAliveObjects_ShouldPassListToHandlers()
        {
            // Arrange
            var handler = new Mock<IVerificationHandler>();

            var trackingObjectReleased = new Mock<ITrackingObject>();
            var releasedObjects = new List<ITrackingObject>() { trackingObjectReleased.Object };

            var trackingObjectAlive = new Mock<ITrackingObject>();
            var aliveObjects = new List<ITrackingObject>() { trackingObjectAlive.Object };

            var multiHandler = new MultiVerificationHandler(new List<IVerificationHandler>() { handler.Object });

            // Act
            multiHandler.VerificationFailed(releasedObjects, aliveObjects);

            // Assert
            handler.Verify(
                (h) =>
                h.VerificationFailed(
                    It.Is<IEnumerable<ITrackingObject>>(
                        (value) =>
                        !object.ReferenceEquals(value, releasedObjects)
                        && value.First() == trackingObjectReleased.Object),
                    It.Is<IEnumerable<ITrackingObject>>(
                        (value) =>
                        !object.ReferenceEquals(value, releasedObjects) 
                        && value.First() == trackingObjectAlive.Object)),
                Times.Once());
        }
    }
}