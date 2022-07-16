using System.Collections.Generic;
using Interfaces;

public class SimpleTile : ITile
{
    public SimpleTile[] myNextTiles;

    private void Start()
    {
        NextTiles = new List<ITile>(myNextTiles);
        
        PrevTiles = new();
        foreach (var tile in NextTiles)
        {
            tile.PrevTiles.Add(this);
        }
    }

    public override bool Fight(List<ICharacter> defenders, ICharacter attacker)
    {
        throw new System.NotImplementedException();
    }
}