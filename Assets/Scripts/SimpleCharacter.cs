using Interfaces;
using UnityEngine;

public class SimpleCharacter : ICharacter
{
    public override void AnimateMovement(ITile tile, float t)
    {
        throw new System.NotImplementedException();
    }

    public override void MoveOneStep(bool onlyVisiting, int moveToTileId)
    {
        throw new System.NotImplementedException();
    }

    public override void TryKill()
    {
        Destroy(gameObject);
    }

    public override bool CheckForCrossroads()
    {
        throw new System.NotImplementedException();
    }
}