using UnityEngine;

namespace Interfaces
{
    public abstract class ICharacter : MonoBehaviour
    {
        public string Name { get; } = "Unnamed Character";
        public ITile CurrentTile { get; protected set; }

        public Team Team { get; protected set; } = new("test0", 0);
        public bool Shield { get; protected set; }
        public bool MustLeave { get; set; }

        public GameObject ConfirmationCanvas { get; set; }

        public abstract void AnimateMovement(ITile tile, float t);
        public abstract void MoveOneStep(bool onlyVisiting, int moveToTileId);
        public abstract void TryKill();
        public abstract bool CheckForCrossroads();
        public abstract void GiveShield();
    }
}