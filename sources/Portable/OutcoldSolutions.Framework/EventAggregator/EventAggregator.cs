// --------------------------------------------------------------------------------------------------------------------
// OutcoldSolutions (http://outcoldsolutions.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    /// <summary>
    /// The event aggregator.
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        private readonly ISubject<object> subject = new Subject<object>();

        /// <inheritdoc />
        public IObservable<TEvent> GetEvent<TEvent>()
        {
            return this.subject.OfType<TEvent>().AsObservable();
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent sampleEvent)
        {
            this.subject.OnNext(sampleEvent);
        }
    }
}