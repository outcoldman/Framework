//-----------------------------------------------------------------------------
// Outcold Solution. 
//-----------------------------------------------------------------------------

namespace OutcoldSolutions.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The multi verification handler.
    /// </summary>
    internal class MultiVerificationHandler : IVerificationHandler
    {
        private readonly List<IVerificationHandler> verificationHandlers = new List<IVerificationHandler>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiVerificationHandler"/> class.
        /// </summary>
        /// <param name="verificationHandlers">
        /// The verification handlers.
        /// </param>
        internal MultiVerificationHandler(IEnumerable<IVerificationHandler> verificationHandlers = null)
        {
            if (verificationHandlers != null)
            {
                this.verificationHandlers.AddRange(verificationHandlers);
            }
        }

        /// <summary>
        /// Gets the verification handlers.
        /// </summary>
        internal List<IVerificationHandler> VerificationHandlers
        {
            get
            {
                return this.verificationHandlers;
            }
        }

        /// <inheritdoc />
        public void VerificationSucceeded(IEnumerable<ITrackingObject> releasedObjects)
        {
            if (releasedObjects == null)
            {
                throw new ArgumentNullException("releasedObjects");
            }

            var listOfReleasedObjects = releasedObjects.ToList();

            foreach (var verificationHandler in this.verificationHandlers)
            {
                verificationHandler.VerificationSucceeded(listOfReleasedObjects.AsEnumerable());
            }
        }

        /// <inheritdoc />
        public void VerificationFailed(IEnumerable<ITrackingObject> releasedObjects, IEnumerable<ITrackingObject> aliveObjects)
        {
            if (releasedObjects == null)
            {
                throw new ArgumentNullException("releasedObjects");
            }

            if (aliveObjects == null)
            {
                throw new ArgumentNullException("aliveObjects");
            }

            var listOfReleasedObjects = releasedObjects.ToList();
            var listOfAliveObjects = aliveObjects.ToList();

            foreach (var verificationHandler in this.verificationHandlers)
            {
                verificationHandler.VerificationFailed(listOfReleasedObjects.AsEnumerable(), listOfAliveObjects.AsEnumerable());
            }
        }
    }
}