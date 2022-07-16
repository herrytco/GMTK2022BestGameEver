using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;


public class SimpleTile : ITile
{
    public SimpleTile[] myNextTiles;
    public GameObject LinePrefab;

    private void Start()
    {
        NextTiles = new List<ITile>(myNextTiles);
        
        PrevTiles = new();
        foreach (var tile in NextTiles)
        {
            tile.PrevTiles.Add(this);
        }
        
        DrawConnections();
    }

    private void DrawConnections()
    {
        if(NextTiles == null) return;
        foreach (var tile in NextTiles)
        {
            Vector3[] points = new[] { transform.position, tile.transform.position };
            GameObject linePrefab = Instantiate(LinePrefab, gameObject.transform);
            LineRenderer lineRenderer = linePrefab.GetComponent<LineRenderer>();
            lineRenderer.SetPositions(points);
        }
    }

    public override bool Fight(List<ICharacter> defenders, ICharacter attacker)
    {
        throw new System.NotImplementedException();
    }
}