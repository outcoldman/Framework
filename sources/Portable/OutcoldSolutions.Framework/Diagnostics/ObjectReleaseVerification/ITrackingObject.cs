//-----------------------------------------------------------------------------
// Outcold Solution. 
//-----------------------------------------------------------------------------

namespace OutcoldSolutions.Diagnostics
{
    using System;

    /// <summary>
    /// Interface contains information about tracking object.
    /// </summary>
    public interface ITrackingObject
    {
        /// <summary>
        /// Gets the tracking object instance. Returns <value>null</value> if object is released.
        /// </summary>
        object Instance { get; }

        /// <summary>
        /// Gets a value indicating whether is tracking object alive or released.
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Gets the tracking object type.
        /// </summary>
        Type InstanceType { get; }

        /// <summary>
        /// Gets the context with which this objects was added to verifier. Can be <value>null</value>.
        /// </summary>
        string Context { get; }

        /// <summary>
        /// Gets a date when this object was added to verifier.
        /// </summary>
        DateTime TrackingStartedDate { get; }

        /// <summary>
        /// Gets the additional custom info which can be set when object was added to verifier.
        /// </summary>
        string AdditionalInfo { get; }
    }
}