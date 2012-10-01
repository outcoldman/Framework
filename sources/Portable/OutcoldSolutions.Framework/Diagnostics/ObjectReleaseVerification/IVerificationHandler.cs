//-----------------------------------------------------------------------------
// Outcold Solution. 
//-----------------------------------------------------------------------------

namespace OutcoldSolutions.Diagnostics
{
    using System.Collections.Generic;

    /// <summary>
    /// The VerificationHandler interface. 
    /// </summary>
    public interface IVerificationHandler
    {
        /// <summary>
        /// The verification succeeded.
        /// </summary>
        /// <param name="releasedObjects">
        /// The released Objects.
        /// </param>
        void VerificationSucceeded(IEnumerable<ITrackingObject> releasedObjects);

        /// <summary>
        /// The verificaion failed.
        /// </summary>
        /// <param name="releasedObjects">
        /// The released Objects.
        /// </param>
        /// <param name="aliveObjects">
        /// The alive Objects.
        /// </param>
        void VerificationFailed(IEnumerable<ITrackingObject> releasedObjects, IEnumerable<ITrackingObject> aliveObjects);
    }
}