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
        /// A character passes through this tile (does not stay/try to occupy)
        /// </summary>
        /// <param name="visitor"></param>
        public abstract void Visit(ICharacter visitor);

        /// <summary>
        /// Called when a character tries to occupy this tile, but it already is occupied.
        /// </summary>
        /// <param name="defenders">The characters already present on this tile.
        /// Assumed to be non-empty.</param>
        /// <param name="attacker">The visiting character.</param>
        /// <returns>True if the attacker wins and occupies the tile.</returns>
        public abstract bool Fight(List<ICharacter> defenders, ICharacter attacker);
        
        /// <summary>
        /// A character tries to occupy this tile.
        /// </summary>
        /// <param name="attacker"></param>
        public void Occupy(ICharacter attacker)
        {
            // if the tile is occupied and the visitor loses the fight, do not process effects
            if (Characters.Count != 0 && !Fight(Characters, attacker))
            {
                return;
            }

            foreach (var effect in _activeEffects)
            {
                if (!effect.OnCharacterVisit(this, attacker))
                {
                    return;
                }
            }
        }
    }
}