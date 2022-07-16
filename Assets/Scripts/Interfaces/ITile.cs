using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TileEffects;
using UnityEngine;

namespace Interfaces
{
    [RequireComponent(typeof(EventManager<TileEvent>))]
    public abstract class ITile : MonoBehaviour
    {
        public List<ITile> NextTiles { get; protected set; } = new();
        public List<ITile> PrevTiles { get; protected set; } = new();

        public List<ICharacter> Characters
        {
            get => _characters;
            protected set => _characters = value;
        }

        private List<ICharacter> _characters = new();

        protected EventManager<TileEvent> EventManager
        {
            get
            {
                if (!ReferenceEquals(_eventManager, null)) _eventManager = GetComponent<EventManager<TileEvent>>();
                return _eventManager;
            }
        }

        private EventManager<TileEvent> _eventManager;

        public void AddObserver([NotNull] IEventObserver<TileEvent> obs) => EventManager.RegisterObserver(obs);
        public void RemoveObserver([NotNull] IEventObserver<TileEvent> obs) => EventManager.DeregisterObserver(obs);

        /// <summary>
        ///     A character passes through this tile (does not stay/try to occupy)
        /// </summary>
        public void Visit(ICharacter visitor, [NotNull] Action<ITile, ICharacter> onDone)
        {
            EventManager.Emit(new TileVisitEvent(this, visitor, true), () => onDone(this, visitor));
        }

        /// <summary>
        ///     A character tries to occupy this tile.
        /// </summary>
        public void Occupy(ICharacter attacker, [NotNull] Action<ITile, ICharacter> onDone)
        {
            EventManager.Emit(new TileVisitEvent(this, attacker, false), () => onDone(this, attacker));
        }

        /// <summary>
        ///     A character (that has occupied this tile previously) leaves this tile.
        /// </summary>
        public void Leave(ICharacter character, ITile destination, [NotNull] Action<ITile, ICharacter> onDone)
        {
            EventManager.Emit(new TileLeaveEvent(this, character), () =>
            {
                _characters = new();
                onDone(this, character);
            });
        }
    }
}