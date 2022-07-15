using System.Collections.Generic;

namespace Interfaces
{
    public abstract class IBoard
    {
        public List<ITile> Tiles { get; private set; }

        public List<ITile> SpawnPoints { get; private set; }
    }
}