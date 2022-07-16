using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

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
        
        foreach (var tile in NextTiles)
        {
            Debug.DrawLine(this.transform.position, tile.transform.position, Color.red, 10000000000);
        }
    }

    public override bool Fight(List<ICharacter> defenders, ICharacter attacker)
    {
        throw new System.NotImplementedException();
    }
}