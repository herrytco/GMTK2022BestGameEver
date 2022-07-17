using System;
using Events;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    public class Mana : MonoBehaviour, IEventObserver<TileEvent>
    {
        public int Priority => 3;
        public bool isDone { get; private set; } = false;
        public bool deregisterWhenDone => true;

        public float effectDuration = 2f;

        private Action _onDone;

        public void OnEvent(TileEvent evnt, Action onDone)
        {
            if (evnt is not TileVisitEvent tileVisitEvent) return;
            if (tileVisitEvent.PassThrough) return;

            _onDone = onDone;
            _pickUpEffectActive = true;
            _pickUpEffectStart = Time.time;

            var wiggle = GetComponent<UpDownBumper>();
            if (!ReferenceEquals(wiggle, null))
            {
                wiggle.enabled = false;
                wiggle.Reset();
            }
        }

        private bool _pickUpEffectActive = false;
        private float _pickUpEffectStart;

        private void Update()
        {
            if (!_pickUpEffectActive) return;

            transform.Translate(0, .1f * Time.deltaTime, 0);

            if (Time.time - _pickUpEffectStart >= effectDuration)
            {
                // TODO give character mana
                print("mana count not implemented lol");

                _pickUpEffectActive = false;
                isDone = true;
                _onDone();
                enabled = false;
                Destroy(gameObject);
            }
        }
    }
}