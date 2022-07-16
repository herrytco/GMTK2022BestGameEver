using System;
using Events;
using Interfaces;
using UnityEngine;

namespace Items
{
    public class Mana : MonoBehaviour, IEventObserver<TileEvent>
    {
        public int Priority => 3;
        public bool isDone { get; private set; } = false;
        public bool deregisterWhenDone => true;
        public GameObject destroyWhenDone { get; private set; } = null;

        private Action _onDone;
        
        public void OnEvent(TileEvent evnt, Action onDone)
        {
            if (evnt is not TileVisitEvent tileVisitEvent) return;
            if (tileVisitEvent.PassThrough) return;

            _onDone = onDone;
            pickUpEffectActive = true;
            
            // TODO give character mana
            print("mana count not implemented lol");
            isDone = true; // remove this to wait until onDone is called
            destroyWhenDone = gameObject;
        }

        private bool pickUpEffectActive = false;

        private void Update()
        {
            if (!pickUpEffectActive) return;
            
            // TODO do something over time, when its done call "_onDone"
        }
    }
}