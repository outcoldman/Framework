//-----------------------------------------------------------------------------
// Outcold Solution. 
//-----------------------------------------------------------------------------

namespace OutcoldSolutions.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The verifier implementation.
    /// </summary>
    internal class VerifierImplementation
    {
        private readonly Dictionary<string, List<TrackingObject>> trackingObjects = new Dictionary<string, List<TrackingObject>>();
        private readonly object verificationLock = new object();
        private readonly IVerificationHandler verificationHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerifierImplementation"/> class.
        /// </summary>
        /// <param name="verificationHandler">
        /// The verification handler.
        /// </param>
        internal VerifierImplementation(IVerificationHandler verificationHandler)
        {
            if (verificationHandler == null)
            {
                throw new ArgumentNullException("verificationHandler");
            }

            this.verificationHandler = verificationHandler;
            this.ForceGarbageCollection = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether verification methods should force garbage collection to collect.
        /// </summary>
        /// <remarks>
        /// Default value is <value>true</value>.
        /// </remarks>
        internal bool ForceGarbageCollection { get; set; } 

        /// <summary>
        /// Track object.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="trackingObject">
        /// The trackingObject.
        /// </param>
        /// <param name="objectInfo">
        /// Additional object info.
        /// </param>
        /// <remarks>
        /// Thread safe. 
        /// </remarks>
        public void TrackObject(string context, object trackingObject, string objectInfo = null)
        {
            Type type = trackingObject.GetType();

            lock (this.verificationLock)
            {
                List<TrackingObject> objects;
                if (!this.trackingObjects.TryGetValue(context, out objects))
                {
                    objects = new List<TrackingObject>();
                    this.trackingObjects.Add(context, objects);
                }

                objects.Add(new TrackingObject(new WeakReference(trackingObject), type, context, objectInfo));
            }
        }

        /// <summary>
        /// Remove object from tracking collection
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="trackingObject">
        /// The tracking object.
        /// </param>
        /// <remarks>
        /// Thread safe. 
        /// </remarks>
        public void RemoveObject(string context, object trackingObject)
        {
            if (trackingObject == null)
            {
                throw new ArgumentNullException("trackingObject");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrEmpty(context))
            {
                throw new ArgumentException(ObjectReleaseVerificationResources.ErrMsg_ContextCannotBeEmpty, "context");
            }

            lock (this.verificationLock)
            {
                List<TrackingObject> objects;
                if (this.trackingObjects.TryGetValue(context, out objects))
                {
                    TrackingObject trackingObjectToRemove = null;

                    foreach (var trackingObjectInfo in objects)
                    {
                        if (trackingObjectInfo.IsAlive 
                            && trackingObjectInfo.Instance != null 
                            && object.ReferenceEquals(trackingObject, trackingObjectInfo.Instance))
                        {
                            trackingObjectToRemove = trackingObjectInfo;
                            break;
                        }
                    }

                    if (trackingObjectToRemove != null)
                    {
                        objects.Remove(trackingObjectToRemove);
                    }
                }
            }
        }

        /// <summary>
        /// Verify objects in context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <remarks>
        /// Thread safe.
        /// </remarks>
        public void Verify(string context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrEmpty(context))
            {
                throw new ArgumentException(ObjectReleaseVerificationResources.ErrMsg_ContextCannotBeEmpty, "context");
            }

            var releasedObjects = new List<TrackingObject>();
            var aliveObjects = new List<TrackingObject>();

            lock (this.verificationLock)
            {
                if (this.ForceGarbageCollection)
                {
                    GC.Collect();
                }

                List<TrackingObject> objects;
                if (this.trackingObjects.TryGetValue(context, out objects))
                {
                    this.VerifyContext(context, objects, releasedObjects, aliveObjects);
                }
            }

            this.HandleVerificationResult(releasedObjects, aliveObjects);
        }

        /// <summary>
        /// Verify objects in <paramref name="context"/> with delay in <paramref name="delay"/> milliseconds.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="delay">
        /// The verification delay.
        /// </param>
        /// <remarks>
        /// Thread safe.
        /// </remarks>
        public void Verify(string context, int delay)
        {
            this.VerifyOnDelay(delay, () => this.Verify(context));
        }

        /// <summary>
        /// Verify all tracking objects.
        /// </summary>
        public void VerifyAll()
        {
            var releasedObjects = new List<TrackingObject>();
            var aliveObjects = new List<TrackingObject>();

            lock (this.verificationLock)
            {
                if (this.ForceGarbageCollection)
                {
                    GC.Collect();
                }

                foreach (var context in this.trackingObjects)
                {
                    this.VerifyContext(context.Key, context.Value, releasedObjects, aliveObjects);
                }

                Monitor.Exit(this.verificationLock);
            }

            this.HandleVerificationResult(releasedObjects, aliveObjects);
        }

        /// <summary>
        /// Verify all tracking objects with delay in <paramref name="delay"/> milliseconds.
        /// </summary>
        /// <param name="delay">
        /// The verification delay.
        /// </param>
        public void VerifyAll(int delay)
        {
            this.VerifyOnDelay(delay, this.VerifyAll);
        }

        private async void VerifyOnDelay(int delay, Action action)
        {
            await Task.Delay(delay);

            action();
        }

        private void HandleVerificationResult(List<TrackingObject> releasedObjects, List<TrackingObject> aliveObjects)
        {
            if (aliveObjects.Count > 0)
            {
                this.verificationHandler.VerificationFailed(releasedObjects.Cast<ITrackingObject>().AsEnumerable(), aliveObjects.Cast<ITrackingObject>().AsEnumerable());
            }
            else
            {
                this.verificationHandler.VerificationSucceeded(releasedObjects.Cast<ITrackingObject>().AsEnumerable());
            }
        }

        private void VerifyContext(string context, List<TrackingObject> objects, List<TrackingObject> releasedObjects, List<TrackingObject> aliveObjects)
        {
            var contextAliveObjects = new List<TrackingObject>();

            foreach (var trackingObject in objects)
            {
                if (trackingObject.IsAlive)
                {
                    aliveObjects.Add(trackingObject);
                }
                else
                {
                    contextAliveObjects.Add(trackingObject);
                    releasedObjects.Add(trackingObject);
                }
            }

            if (contextAliveObjects.Count > 0)
            {
                this.trackingObjects[context] = contextAliveObjects;
            }
            else
            {
                this.trackingObjects.Remove(context);
            }
        }
    }
}