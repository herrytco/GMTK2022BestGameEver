using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        private IEnumerator _handleEventsCoroutine;

        public void Emit(TEvent evnt, Action onDone)
        {
            if (_handleEventsCoroutine != null) StopCoroutine(_handleEventsCoroutine);

            _toDoEvent = evnt;
            _onDone = onDone;
            _handleEventsCoroutine = HandleEventsCoroutine();
            StartCoroutine(_handleEventsCoroutine);
        }

        private bool _waitingForEventObserver;

        protected IEnumerator HandleEventsCoroutine()
        {
            Debug.Log($"EventManager (on {gameObject.name}): started for {_toDoEvent}");
            _toDoObservers = Observers;
            while (_toDoObservers.Count != 0)
            {
                var obs = _toDoObservers[0];
                _toDoObservers.RemoveAt(0);

                _waitingForEventObserver = true;
                obs.OnEvent(_toDoEvent, () => _waitingForEventObserver = false);
                if (_waitingForEventObserver) Debug.Log($"EventManager (on {gameObject.name}): waiting for {obs} to handle {_toDoEvent}");
                while (_waitingForEventObserver) yield return null;
                Debug.Log($"EventManager (on {gameObject.name}): event handled by {obs}");
                if (obs.deregisterWhenDone) DeregisterObserver(obs);
            }

            Debug.Log($"EventManager (on {gameObject.name}): done for {_toDoEvent}, calling onDone");
            _handleEventsCoroutine = null;
            _onDone();
        }
    }
}