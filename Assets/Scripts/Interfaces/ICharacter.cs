using UnityEngine;

namespace Interfaces
{
    public abstract class ICharacter : MonoBehaviour
    {
        public string Name { get; } = "Unnamed Character";
        public ITile CurrentTile { get; private set; }

        public Team Team { get; } = new("test0", 0);
        public bool Shield { get; private set; }

        public GameObject ConfirmationCanvas { get; set; }

        public abstract void AnimateMovement(ITile tile, float t);
    }
}