//-----------------------------------------------------------------------------
// Outcold Solution. 
//-----------------------------------------------------------------------------

namespace OutcoldSolutions.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Implementation of <see cref="IVerificationHandler"/> which outputs information about 
    /// released and alive objects to method passed in constructor.
    /// </summary>
    /// <example>
    /// Below example how to create and register new instance of <see cref="IVerificationHandler"/> which will write
    /// output to debug console only in debug mode:
    /// <code>
    /// var verificationHandler = new TextOutputVerificationHandler((messageLine) => Debug.WriteLine(messageLine));
    /// ObjectReleaseVerifier.AddVerificationHandler(verificationHandler);
    /// </code>
    /// </example>
    public class TextOutputVerificationHandler : IVerificationHandler
    {
        private readonly Action<string> writeLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextOutputVerificationHandler"/> class.
        /// </summary>
        /// <param name="writeLine">
        /// The output function, which can write output message lines.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="writeLine"/> will throw exception.
        /// </exception>
        public TextOutputVerificationHandler(Action<string> writeLine)
        {
            if (writeLine == null)
            {
                throw new ArgumentNullException("writeLine");
            }

            this.writeLine = writeLine;
        }

        /// <inheritdoc />
        public void VerificationSucceeded(IEnumerable<ITrackingObject> releasedObjects)
        {
            this.writeLine(string.Empty);
            this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.VerificationSuccededTitle, DateTime.Now));
            this.writeLine(string.Empty);

            this.WriteReleasedObjectInfos(releasedObjects);
        }

        /// <inheritdoc />
        public void VerificationFailed(IEnumerable<ITrackingObject> releasedObjects, IEnumerable<ITrackingObject> aliveObjects)
        {
            this.writeLine(string.Empty);
            this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.VerificationFailedTitle, DateTime.Now));
            this.writeLine(string.Empty);
            
            this.WriteAliveObjectInfos(aliveObjects);
            this.WriteReleasedObjectInfos(releasedObjects);
        }

        private void WriteReleasedObjectInfos(IEnumerable<ITrackingObject> trackingObjects)
        {
            int releasedObjectsCount = 0;

            foreach (var trackingObject in trackingObjects)
            {
                if (releasedObjectsCount == 0)
                {
                    this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.ReleasedObjectsTitle, DateTime.Now));
                }

                this.WriteObjectInfo(trackingObject);

                releasedObjectsCount++;
            }

            if (releasedObjectsCount > 0)
            {
                this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.ReleasedObjectsEnd, releasedObjectsCount));
                this.writeLine(string.Empty);
            }
        }

        private void WriteAliveObjectInfos(IEnumerable<ITrackingObject> trackingObjects)
        {
            int aliveObjectsCount = 0;

            foreach (var trackingObject in trackingObjects)
            {
                if (aliveObjectsCount == 0)
                {
                    this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.AliveObjectsTitle, DateTime.Now));
                }

                this.WriteObjectInfo(trackingObject);

                aliveObjectsCount++;
            }

            if (aliveObjectsCount > 0)
            {
                this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.AliveObjectsEnd, aliveObjectsCount));
                this.writeLine(string.Empty);
            }
        }

        private void WriteObjectInfo(ITrackingObject trackingObject)
        {
            this.writeLine(ObjectReleaseVerificationResources.Output_Separator);
            this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.ObjectInfo_Object, trackingObject.Instance));
            this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.ObjectInfo_IsAlive, trackingObject.IsAlive));
            this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.ObjectInfo_Type, trackingObject.InstanceType.FullName));
            this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.ObjectInfo_Context, trackingObject.Context));
            this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.ObjectInfo_TrackingStartedDate, trackingObject.TrackingStartedDate));
            this.writeLine(string.Format(CultureInfo.CurrentCulture, ObjectReleaseVerificationResources.ObjectInfo_AdditionalInfo, trackingObject.AdditionalInfo));
            this.writeLine(ObjectReleaseVerificationResources.Output_Separator);
        }
    }
}