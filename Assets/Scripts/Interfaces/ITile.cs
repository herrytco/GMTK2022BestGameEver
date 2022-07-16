using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Interfaces
{
    public abstract class ITile : MonoBehaviour
    {
        protected List<ITileEffect> _activeEffects;
        public List<ITile> NextTiles { get; protected set; } = new();
        public List<ITile> PrevTiles { get; protected set; } = new();

        public List<ICharacter> Characters { get; protected set; } = new();

        public List<ITileEffect> ActiveEffects => _activeEffects;

        public void AddEffect(ITileEffect effect)
        {
            _activeEffects.Add(effect);
            // TODO check if this sorts the right way around
            _activeEffects.Sort(Comparer<ITileEffect>.Create((a, b) =>
                -a.Priority.CompareTo(b.Priority)));
        }

        public void RemoveEffectsWhere(Predicate<ITileEffect> match)
        {
            _activeEffects.RemoveAll(match);
        }

        /// <summary>
        ///     A character passes through this tile (does not stay/try to occupy)
        /// </summary>
        public void Visit(ICharacter visitor, [NotNull] Action<ITile, ICharacter> onDone)
        {
            foreach (var effect in _activeEffects.ToList()) effect.OnCharacterVisit(this, visitor);

            onDone(this, visitor);
        }

        /// <summary>
        ///     Called when a character tries to occupy this tile, but it already is occupied.
        /// </summary>
        /// <param name="defenders">
        ///     The characters already present on this tile.
        ///     Assumed to be non-empty.
        /// </param>
        /// <param name="attacker">The visiting character.</param>
        /// <returns>True if the attacker wins and occupies the tile.</returns>
        public abstract bool Fight(List<ICharacter> defenders, ICharacter attacker);

        /// <summary>
        ///     A character tries to occupy this tile.
        /// </summary>
        public void Occupy(ICharacter attacker, [NotNull] Action<ITile, ICharacter> onDone)
        {
            // if the tile is occupied and the visitor loses the fight, do not process effects
            if (Characters.Count != 0 && !Fight(Characters, attacker))
            {
                onDone(this, attacker);
                return;
            }

            foreach (var effect in _activeEffects.ToList())
                if (!effect.OnOccupied(this, attacker))
                {
                    onDone(this, attacker);
                    return;
                }

            onDone(this, attacker);
        }

        /// <summary>
        ///     A character (that has occupied this tile previously) leaves this tile.
        /// </summary>
        public void Leave(ICharacter character, ITile destination, [NotNull] Action<ITile, ICharacter> onDone)
        {
            foreach (var effect in _activeEffects.ToList())
                if (!effect.OnLeave(this, character, destination))
                {
                    onDone(this, character);
                    return;
                }

            Characters.RemoveAll(c => c == character);

            onDone(this, character);
        }
    }
}