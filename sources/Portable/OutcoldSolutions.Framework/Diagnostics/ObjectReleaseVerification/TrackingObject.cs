//-----------------------------------------------------------------------------
// Outcold Solution. 
//-----------------------------------------------------------------------------

namespace OutcoldSolutions.Diagnostics
{
    using System;

    /// <summary>
    /// Tracking object implementation.
    /// </summary>
    internal class TrackingObject : ITrackingObject
    {
        private readonly WeakReference weakReference;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingObject"/> class.
        /// </summary>
        /// <param name="weakReference">
        /// The weak reference.
        /// </param>
        /// <param name="trackingObjectType">
        /// The tracking object type.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="additionalInfo">
        /// The additional info.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the <paramref name="weakReference"/> is null.
        /// </exception>
        internal TrackingObject(WeakReference weakReference, Type trackingObjectType, string context, string additionalInfo)
        {
            if (weakReference == null)
            {
                throw new ArgumentNullException("weakReference");
            }

            this.Context = context;
            this.weakReference = weakReference;
            this.InstanceType = trackingObjectType;
            this.AdditionalInfo = additionalInfo;
            this.TrackingStartedDate = DateTime.Now;
        }

        /// <inheritdoc />
        public object Instance 
        { 
            get
            {
                return this.weakReference.Target;
            }
        }

        /// <inheritdoc />
        public bool IsAlive
        {
            get
            {
                return this.weakReference.IsAlive;
            }
        }

        /// <inheritdoc />
        public Type InstanceType { get; private set; }

        /// <inheritdoc />
        public string Context { get; private set; }

        /// <inheritdoc />
        public DateTime TrackingStartedDate { get; private set; }

        /// <inheritdoc />
        public string AdditionalInfo { get; private set; }
    }
}