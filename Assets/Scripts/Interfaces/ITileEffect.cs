namespace Interfaces
{
    public interface ITileEffect
    {
        /// <summary>
        /// Higher priority means this effect will get applied sooner.
        /// The priority is expected not to change at run-time.
        /// </summary>
        public int Priority { get; }

        /// <returns>True if the character stays and remaining effects should be applied.</returns>
        public bool OnCharacterVisit(ITile tile, ICharacter visitor) => true;
    }
}