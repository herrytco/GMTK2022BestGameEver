namespace Interfaces
{
    public abstract class TileEvent
    {
        public readonly ITile Tile;

        protected TileEvent(ITile tile)
        {
            Tile = tile;
        }
    }

    public class TileVisitEvent : TileEvent
    {
        public readonly ICharacter Character;

        /// <summary>
        /// True if the character is just passing though and will not stay on/attack the tile.
        /// </summary>
        public readonly bool PassThrough;

        /// <summary>
        ///     A character visits (arrives at) a tile.
        /// </summary>
        public TileVisitEvent(ITile tile, ICharacter character, bool passThrough) : base(tile)
        {
            Character = character;
            PassThrough = passThrough;
        }
    }

    public class TileLeaveEvent : TileEvent
    {
        public readonly ICharacter Character;

        /// <summary>
        ///     A character leaves (starts moving from) a tile (not just passed through).
        /// </summary>
        public TileLeaveEvent(ITile tile, ICharacter character) : base(tile)
        {
            Character = character;
        }
    }
}