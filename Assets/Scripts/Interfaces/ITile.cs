using System.Collections.Generic;
using Interfaces;

namespace Interfaces
{
    public abstract class ITile
    {
        public List<ITile> NextTiles { get; private set; } = new();
        public List<ITile> PrevTiles { get; private set; } = new();
        
        public List<ICharacter> Characters { get; private set; } = new();
    }
}
