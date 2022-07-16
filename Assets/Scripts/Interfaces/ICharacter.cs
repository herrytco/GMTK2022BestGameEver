using UnityEngine;

namespace Interfaces
{
    public abstract class ICharacter : MonoBehaviour
    {
        public string Name { get; } = "Unnamed Character";
        public ITile CurrentTile { get; protected set; }

        public Team Team { get; } = new("test0", 0);
        public bool Shield { get; protected set; }

        public GameObject ConfirmationCanvas { get; set; }

        public abstract void Kill();
    }
}