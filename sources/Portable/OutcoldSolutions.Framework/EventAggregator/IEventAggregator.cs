// --------------------------------------------------------------------------------------------------------------------
// OutcoldSolutions (http://outcoldsolutions.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The Event Aggregator interface.
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// Get event.
        /// </summary>
        /// <typeparam name="TEvent">
        /// Type of event.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IObservable{TEvent}"/>.
        /// </returns>
        IObservable<TEvent> GetEvent<TEvent>();

        /// <summary>
        /// Publish with type <typeparamref name="TEvent"/>.
        /// </summary>
        /// <param name="sampleEvent">
        /// The sample event.
        /// </param>
        /// <typeparam name="TEvent">
        /// Event type.
        /// </typeparam>
        void Publish<TEvent>(TEvent sampleEvent);
    }
}
