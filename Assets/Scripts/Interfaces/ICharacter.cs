using System;

namespace Interfaces
{
    public abstract class ICharacter
    {
        public String Name { get; private set; } = "Unnamed Character";
        public ITile CurrentTile { get; private set; }

        // public Object Team { get; private set; }
    }
}