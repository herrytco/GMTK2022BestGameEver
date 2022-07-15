namespace Interfaces
{
    public interface ITileEffect
    {
        /// <summary>
        /// Higher priority means this effect will get applied sooner.
        /// The priority is expected not to change at run-time.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Called after a character has occupied this tile.
        /// </summary>
        /// <returns>True if the character stays and remaining effects should be applied.</returns>
        public bool OnOccupied(ITile tile, ICharacter visitor) => true;

        /// <summary>
        /// Called when a character (that has occupied this tile) leaves this tile.
        /// </summary>
        /// <returns>True if nothing happens to the character and remaining effects should be applied.</returns>
        public bool OnLeave(ITile tile, ICharacter visitor, ITile destination) => true;

        /// <summary>
        /// Called when a character passes through this tile.
        /// </summary>
        /// <returns>True if nothing happens to the character and remaining effects should be applied.</returns>
        public bool OnCharacterVisit(ITile tile, ICharacter visitor) => true;
    }
}