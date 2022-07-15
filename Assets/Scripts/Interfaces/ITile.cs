using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public abstract class ITile : MonoBehaviour
    {
        public List<ITile> NextTiles { get; private set; } = new();
        public List<ITile> PrevTiles { get; private set; } = new();
        
        public List<ICharacter> Characters { get; private set; } = new();
    }
}
