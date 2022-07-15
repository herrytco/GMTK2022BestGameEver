using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public abstract class ITile : MonoBehaviour
    {
        public List<ITile> NextTiles { get; private set; } = new();
        public List<ITile> PrevTiles { get; private set; } = new();

        public List<ICharacter> Characters { get; private set; } = new();

        private List<ITileEffect> _activeEffects;

        public void AddEffect(ITileEffect effect)
        {
            _activeEffects.Add(effect);
            // TODO check if this sorts the right way around
            _activeEffects.Sort(Comparer<ITileEffect>.Create((a, b) =>
                -a.Priority.CompareTo(b.Priority)));
        }

        /// <summary>
        /// Called when a character visits this tile, but it already is occupied.
        /// </summary>
        /// <param name="defenders">The characters already present on this tile.
        /// Assumed to be non-empty.</param>
        /// <param name="attacker">The visiting character.</param>
        /// <returns>True if the attacker wins and occupies the tile.</returns>
        public abstract bool Fight(List<ICharacter> defenders, ICharacter attacker);

        public void Visit(ICharacter visitor)
        {
            // if the tile is occupied and the visitor loses the fight, do not process effects
            if (Characters.Count != 0 && !Fight(Characters, visitor))
            {
                return;
            }

            foreach (var effect in _activeEffects)
            {
                if (!effect.OnCharacterVisit(this, visitor))
                {
                    return;
                }
            }
        }
    }
}