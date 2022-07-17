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

    public override void Kill()
    {
        Destroy(gameObject);
    }

    public override bool CheckForCrossroads()
    {
        throw new System.NotImplementedException();
    }
}