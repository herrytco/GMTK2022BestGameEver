namespace Interfaces
{
    public abstract class TileEvent
    {
        public readonly GameManager GameManager;
        public readonly ITile Tile;

        protected TileEvent(GameManager gameManager, ITile tile)
        {
            Tile = tile;
            GameManager = gameManager;
        }
    }

    public class TileVisitEvent : TileEvent
    {
        public readonly ICharacter Character;

        /// <summary>
        ///     True if the character is just passing though and will not stay on/attack the tile.
        /// </summary>
        public readonly bool PassThrough;

        /// <summary>
        ///     A character visits (arrives at) a tile.
        /// </summary>
        public TileVisitEvent(GameManager gameManager, ITile tile, ICharacter character, bool passThrough) : base(gameManager, tile)
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
        public TileLeaveEvent(GameManager gameManager, ITile tile, ICharacter character) : base(gameManager, tile) => Character = character;
    }

    public class TileTurnEvent : TileEvent
    {
        public readonly bool IsAtBeginningOfTurn;
        public readonly int TurnCount;
        
        public bool IsAtEndOfTurn => !IsAtBeginningOfTurn;

        /// <summary>
        ///     A game turn ends.
        /// </summary>
        public TileTurnEvent(GameManager gameManager, ITile tile, bool isAtBeginningOfTurn, int turnCount) : base(gameManager, tile)
        {
            IsAtBeginningOfTurn = isAtBeginningOfTurn;
            TurnCount = turnCount;
        }
    }
}