using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

[ExecuteInEditMode]
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

    private void OnDrawGizmos()
    {
        foreach (var tile in NextTiles)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, tile.transform.position);
        }
    }

    public override bool Fight(List<ICharacter> defenders, ICharacter attacker)
    {
        throw new System.NotImplementedException();
    }
}