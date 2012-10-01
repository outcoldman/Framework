//-----------------------------------------------------------------------------
// Outcold Solution. 
//-----------------------------------------------------------------------------

namespace OutcoldSolutions.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The object release verifier. 
    /// This class tracks all objects which were registered by one of the methods TrackObject and 
    /// verifies on Verify for specific context or on VerifyAll for all tracking objects in all contextes
    /// that all of them were released from memory. 
    /// The collections of released and alive tracking objects after verification will be passed to registered by <see cref="AddVerificationHandler"/>
    /// verification handlers (<see cref="IVerificationHandler"/>).
    /// </summary>
    /// <remarks>
    /// By default <see cref="ObjectReleaseVerifier"/> is disabled. Change value of <see cref="Enabled"/> to turn on verification.
    /// There are no default verification handlers are registered. 
    /// It is better to enable object release verification only in debug mode when performance is not so important. Object release verificator
    /// requires to have <see cref="ForceGarbageCollection"/> enabled, which can have impact on application performance because of invoking
    /// <see cref="GC.Collect"/> methods.
    /// </remarks>
    /// <example>
    /// To user verification release verifier you need to setup it first:
    /// <code>
    /// // Verification handler which only with in Debug mode will output tracking objects information to debug console.
    /// var verificationHandler = new TextOutputVerificationHandler((messageLine) => Debug.WriteLine(messageLine));
    /// ObjectReleaseVerifier.AddVerificationHandler(verificationHandler);
    /// ObjectReleaseVerifier.Enable = true;
    /// </code>
    /// </example>
    public static class ObjectReleaseVerifier
    {
        private static readonly MultiVerificationHandler MultiVerificationHandler = new MultiVerificationHandler();
        private static readonly VerifierImplementation Implementation = new VerifierImplementation(MultiVerificationHandler);

        /// <summary>
        /// Gets or sets a value indicating whether verification is enabled or disabled.
        /// </summary>
        /// <remarks>
        /// Default value is <value>false</value>.
        /// </remarks>
        public static bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether verification should force garbage collection.
        /// </summary>
        /// <remarks>
        /// Often calls of <see cref="GC.Collect"/> will have impact on application performance. In other way turning off this settings will not guarantee that 
        /// <see cref="ObjectReleaseVerifier"/> will show the right results of object releases, CLR can still keep objects in heap, but these objects will be deleted
        /// on next Garbage Collection, which means that application will not have any memory leaks. Default value is <value>true</value>.
        /// </remarks>
        public static bool ForceGarbageCollection
        {
            get
            {
                return Implementation.ForceGarbageCollection;
            }

            set
            {
                Implementation.ForceGarbageCollection = value;
            }
        }

        /// <summary>
        /// Get all registered verification handlers.
        /// </summary>
        /// <returns>
        /// Collection of registered verification handlers.
        /// </returns>
        public static IEnumerable<IVerificationHandler> GetVerificationHandlers()
        {
            return MultiVerificationHandler.VerificationHandlers.AsEnumerable();
        }

        /// <summary>
        /// Remove registered verification handler.
        /// </summary>
        /// <remarks>
        /// Do nothing if <paramref name="verificationHandler"/> is not registered in the system.
        /// </remarks>
        /// <param name="verificationHandler">
        /// The verification handler implementation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="verificationHandler"/> is null.
        /// </exception>
        public static void RemoveVerificationHandler(IVerificationHandler verificationHandler)
        {
            if (verificationHandler == null)
            {
                throw new ArgumentNullException("verificationHandler");
            }

            MultiVerificationHandler.VerificationHandlers.Remove(verificationHandler);
        }

        /// <summary>
        /// Register verification handler.
        /// </summary>
        /// <param name="verificationHandler">
        /// The verification handler implementation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="verificationHandler"/> is null.
        /// </exception>
        public static void AddVerificationHandler(IVerificationHandler verificationHandler)
        {
            if (verificationHandler == null)
            {
                throw new ArgumentNullException("verificationHandler");
            }

            MultiVerificationHandler.VerificationHandlers.Add(verificationHandler);
        }

        /// <summary>
        /// Track object.
        /// </summary>
        /// <param name="trackingObject">
        /// The object.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="trackingObject"/> is null.
        /// </exception>
        /// <remarks>
        /// Thread safe. 
        /// </remarks>
        public static void TrackObject(object trackingObject)
        {
            if (trackingObject == null)
            {
                throw new ArgumentNullException("trackingObject");
            }

            if (Enabled)
            {
                Implementation.TrackObject(string.Empty, trackingObject);
            }
        }

        /// <summary>
        /// Track object.
        /// </summary>
        /// <param name="trackingObject">
        /// The object.
        /// </param>
        /// <param name="additionalInfo">
        /// Additional object info. Provide additional info to identify object in verification handler (<see cref="IVerificationHandler"/>).
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="trackingObject"/> or <paramref name="additionalInfo"/> is null.
        /// </exception>
        /// <remarks>
        /// Thread safe method.
        /// </remarks>
        public static void TrackObject(object trackingObject, string additionalInfo)
        {
            if (trackingObject == null)
            {
                throw new ArgumentNullException("trackingObject");
            }

            if (additionalInfo == null)
            {
                throw new ArgumentNullException("additionalInfo");
            }

            if (Enabled)
            {
                Implementation.TrackObject(string.Empty, trackingObject, additionalInfo);
            }
        }

        /// <summary>
        /// Track object.
        /// </summary>
        /// <param name="context">
        /// The context. 
        /// </param>
        /// <param name="trackingObject">
        /// The object.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="context"/> is <value>string.Empty</value> string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="context"/> or <paramref name="trackingObject"/> is null.
        /// </exception>
        /// <remarks>
        /// Thread safe method.
        /// </remarks>
        public static void TrackObject(string context, object trackingObject)
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

            if (Enabled)
            {
                Implementation.TrackObject(context, trackingObject);
            }
        }

        /// <summary>
        /// Track object.
        /// </summary>
        /// <param name="context">
        /// The context. 
        /// </param>
        /// <param name="trackingObject">
        /// The object.
        /// </param>
        /// <param name="additionalInfo">
        /// Additional object info. Provide additional info to identify object in verification handler (<see cref="IVerificationHandler"/>).
        /// </param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="context"/> is <value>string.Empty</value> string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="context"/>, <paramref name="trackingObject"/> or <paramref name="additionalInfo"/> is null.
        /// </exception>
        /// <remarks>
        /// Thread safe method.
        /// </remarks>
        public static void TrackObject(string context, object trackingObject, string additionalInfo)
        {
            if (trackingObject == null)
            {
                throw new ArgumentNullException("trackingObject");
            }

            if (additionalInfo == null)
            {
                throw new ArgumentNullException("additionalInfo");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrEmpty(context))
            {
                throw new ArgumentException(ObjectReleaseVerificationResources.ErrMsg_ContextCannotBeEmpty, "context");
            }

            if (Enabled)
            {
                Implementation.TrackObject(context, trackingObject, additionalInfo);
            }
        }

        /// <summary>
        /// Remove object from tracking collection.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="trackingObject">
        /// The object.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="context"/> is <value>string.Empty</value> string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="context"/> or <paramref name="trackingObject"/> is null.
        /// </exception>
        public static void RemoveTracking(string context, object trackingObject)
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

            if (Enabled)
            {
                Implementation.RemoveObject(context, trackingObject);
            }
        }

        /// <summary>
        /// Verify objects in context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="context"/> is <value>string.Empty</value> string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="context"/> is null.
        /// </exception>
        /// <remarks>
        /// Thread safe method.
        /// </remarks>
        public static void Verify(string context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrEmpty(context))
            {
                throw new ArgumentException(ObjectReleaseVerificationResources.ErrMsg_ContextCannotBeEmpty, "context");
            }

            if (Enabled)
            {
                Implementation.Verify(context);
            }
        }

        /// <summary>
        /// Verify objects in context with delay.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="delay">
        /// Delay in milliseconds.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="context"/> is <value>string.Empty</value> string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="context"/> is null.
        /// </exception>
        /// <remarks>
        /// Thread safe method.
        /// </remarks>
        public static void Verify(string context, int delay)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrEmpty(context))
            {
                throw new ArgumentException(ObjectReleaseVerificationResources.ErrMsg_ContextCannotBeEmpty, "context");
            }

            if (Enabled)
            {
                Implementation.Verify(context, delay);
            }
        }

        /// <summary>
        /// Verify objects in context with delay.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="delay">
        /// The delay.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="context"/> is <value>string.Empty</value> string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="context"/> is null.
        /// </exception>
        /// <remarks>
        /// Thread safe method.
        /// </remarks>
        public static void Verify(string context, TimeSpan delay)
        {
            Verify(context, (int)delay.TotalMilliseconds);
        }

        /// <summary>
        /// Verify all tracking objects.
        /// </summary>
        /// <remarks>
        /// Thread safe method.
        /// </remarks>
        public static void VerifyAll()
        {
            if (Enabled)
            {
                Implementation.VerifyAll();
            }
        }

        /// <summary>
        /// Verify all tracking objects with delay.
        /// </summary>
        /// <param name="delay">
        /// The delay in milliseconds.
        /// </param>
        /// <remarks>
        /// Thread safe method.
        /// </remarks>
        public static void VerifyAll(int delay)
        {
            if (Enabled)
            {
                Implementation.VerifyAll();
            }
        }

        /// <summary>
        /// Verify all tracking objects with delay.
        /// </summary>
        /// <param name="delay">
        /// The delay in milliseconds.
        /// </param>
        /// <remarks>
        /// Thread safe method.
        /// </remarks>
        public static void VerifyAll(TimeSpan delay)
        {
            VerifyAll((int)delay.TotalMilliseconds);
        }
    }
}
