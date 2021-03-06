using System;
using UnityEngine;

namespace Events
{
    public interface IEventObserver<TEvent>
    {
        public int Priority { get; }

        public bool isDone { get; }
        public bool deregisterWhenDone { get; }

        /// <summary>
        ///     Notify of an event.
        ///     Assumed to EITHER: have "isDone = true" when this method returns,
        ///     OR: call onDone when the event has been properly processed.
        /// </summary>
        public void OnEvent(TEvent evnt, Action onDone);
    }
}