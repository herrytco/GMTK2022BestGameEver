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
        public bool deregisterWhenDone { get; private set; } = false;

        public float effectDuration = 2f;

        private Action _onDone;

        private Team _team;
        private GameManager _gameManager;

        public void OnEvent(TileEvent evnt, Action onDone)
        {
            if (evnt is not TileVisitEvent { PassThrough: false } visitEvent)
            {
                // irrelevant event, signal that the EventManager can continue
                isDone = true;
                onDone();
                return;
            }

            _team = visitEvent.Character.Team;
            _gameManager = visitEvent.GameManager;
            
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
                _gameManager.GiveActiveTeamMana();

                // done with animation, signal that the EventManager can continue
                deregisterWhenDone = true; // and dont call me anymore!
                isDone = true;
                _onDone();
                
                // dispose this
                _pickUpEffectActive = false;
                enabled = false;
                Destroy(gameObject);
            }
        }
    }
}