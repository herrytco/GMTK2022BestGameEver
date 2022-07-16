using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public abstract class EventManager<TEvent> : MonoBehaviour
    {
        private List<IEventObserver<TEvent>> _observers = new();

        public List<IEventObserver<TEvent>> Observers
        {
            get => new(_observers);
            protected set => _observers = value;
        }

        public virtual void RegisterObserver(IEventObserver<TEvent> observer)
        {
            var tmpObservers = Observers;
            tmpObservers.Add(observer);
            // TODO check if this sorts the right way around
            tmpObservers.Sort(
                Comparer<IEventObserver<TEvent>>.Create(
                    (a, b) => -a.Priority.CompareTo(b.Priority)
                )
            );
            _observers = tmpObservers;
        }

        public virtual void DeregisterObserver(IEventObserver<TEvent> observer)
        {
            _observers.Remove(observer);
            _toDoObservers.Remove(observer);
        }

        /// <summary>
        ///     List of observers that currently wait to be notified. Only used while an event is processed. 
        /// </summary>
        private List<IEventObserver<TEvent>> _toDoObservers = new();

        private TEvent _toDoEvent;
        private Action _onDone;

        public void Emit(TEvent evnt, Action onDone)
        {
            _toDoEvent = evnt;
            _onDone = onDone;
            StartCoroutine(HandleEvents());
        }

        protected IEnumerator HandleEvents()
        {
            _toDoObservers = Observers;
            while (_toDoObservers.Count != 0)
            {
                var obs = _toDoObservers[0];
                _toDoObservers.RemoveAt(0);

                obs.OnEvent(_toDoEvent, ResumeHandleEvents);
                if (!obs.isDone) yield return null;
                if (obs.deregisterWhenDone) DeregisterObserver(obs);
                if (!ReferenceEquals(obs.destroyWhenDone, null)) Destroy(obs.destroyWhenDone);
            }

            _onDone();
        }

        private Action ResumeHandleEvents
        {
            get { return () => StartCoroutine(HandleEvents()); }
        }
    }
}