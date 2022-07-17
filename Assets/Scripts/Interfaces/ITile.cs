using System;
using System.Collections.Generic;
using Events;
using JetBrains.Annotations;
using UnityEngine;

namespace Interfaces
{
    [RequireComponent(typeof(TileEventManager))]
    public abstract class ITile : MonoBehaviour
    {
        public List<ITile> NextTiles { get; protected set; } = new();
        public List<ITile> PrevTiles { get; protected set; } = new();

        public List<ICharacter> Characters = new();

        protected EventManager<TileEvent> EventManager
        {
            get
            {
                if (ReferenceEquals(_eventManager, null)) _eventManager = GetComponent<EventManager<TileEvent>>();
                return _eventManager;
            }
        }

        private EventManager<TileEvent> _eventManager;

        public void AddObserver([NotNull] IEventObserver<TileEvent> obs) => EventManager.RegisterObserver(obs);
        public void RemoveObserver([NotNull] IEventObserver<TileEvent> obs) => EventManager.DeregisterObserver(obs);

        /// <summary>
        ///     A character passes through this tile (does not stay/try to occupy)
        /// </summary>
        public void Visit(GameManager gameManager, ICharacter visitor, [NotNull] Action<ITile, ICharacter> onDone)
        {
            EventManager.Emit(new TileVisitEvent(gameManager, this, visitor, true), () => onDone(this, visitor));
        }

        /// <summary>
        ///     A character tries to occupy this tile.
        /// </summary>
        public void Occupy(GameManager gameManager, ICharacter attacker, [NotNull] Action<ITile, ICharacter> onDone)
        {
            EventManager.Emit(new TileVisitEvent(gameManager, this, attacker, false), () => onDone(this, attacker));
        }

        /// <summary>
        ///     A character (that has occupied this tile previously) leaves this tile.
        /// </summary>
        public void Leave(GameManager gameManager, ICharacter character, ITile destination, [NotNull] Action<ITile, ICharacter> onDone)
        {
            EventManager.Emit(new TileLeaveEvent(gameManager, this, character), () =>
            {
                Characters = new List<ICharacter>();
                onDone(this, character);
            });
        }
        
        /// <summary>
        ///     A game turn has begun/ended.
        /// </summary>
        public void TurnProgress(GameManager gameManager, int turnNumber, bool isBeginningOfTurn, [NotNull] Action<ITile> onDone)
        {
            EventManager.Emit(new TileTurnEvent(gameManager, this, isBeginningOfTurn, turnNumber), () => onDone(this));
        }
    }
}