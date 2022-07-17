using System;
using System.Linq;
using Events;
using Interfaces;
using UnityEngine;

namespace Items
{
    public class Poison : MonoBehaviour, IEventObserver<TileEvent>
    {
        public int Priority => 2;
        public bool isDone { get; private set; } = true;
        public bool deregisterWhenDone { get; private set; } = false;

        public int ActiveTurns { get; private set; } = 3;

        public void OnEvent(TileEvent evnt, Action onDone)
        {
            if (evnt is not TileTurnEvent { IsAtEndOfTurn: true } turnEndEvent)
            {
                isDone = true;
                onDone();
                return;
            }

            var tile = turnEndEvent.Tile;
            foreach (var character in tile.Characters.ToList())
            {
                print($"killing {character.Name}");
                character.Kill();
            }
            tile.Characters = new();

            
            if (--ActiveTurns != 0)
            {
                isDone = true;
                onDone();
                return;
            }
            
            deregisterWhenDone = true;
            onDone();
            enabled = false;
            Destroy(gameObject);
        }
    }
}