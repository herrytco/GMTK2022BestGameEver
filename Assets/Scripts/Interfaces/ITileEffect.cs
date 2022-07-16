using System;
using UnityEngine;

namespace Interfaces
{
    public class TileEffectResult
    {
        /// <summary>
        /// True if the visiting/attacking character is removed and further effects should not be applied.
        /// </summary>
        public readonly bool CharacterRemoved;

        /// <summary>
        /// True if the effect is completed.
        /// </summary>
        public readonly bool Done;

        public TileEffectResult(bool characterRemoved, bool done)
        {
            CharacterRemoved = characterRemoved;
            Done = done;
        }
    }

    public abstract class ITileEffect : MonoBehaviour
    {
        /// <summary>
        ///     Higher priority means this effect will get applied sooner.
        ///     The priority is expected not to change at run-time.
        /// </summary>
        public abstract int Priority { get; }

        /// <summary>
        ///     Called after a character has occupied this tile.
        /// </summary>
        public virtual TileEffectResult OnOccupied(ITile tile, ICharacter visitor, Action onDone) => new(false, true);

        /// <summary>
        ///     Called when a character (that has occupied this tile) leaves this tile.
        /// </summary>
        public virtual TileEffectResult OnLeave(ITile tile, ICharacter visitor, ITile destination, Action onDone) => new(false, true);

        /// <summary>
        ///     Called when a character passes through this tile.
        /// </summary>
        public virtual TileEffectResult OnVisit(ITile tile, ICharacter visitor, Action onDone) => new(false, true);
    }
}