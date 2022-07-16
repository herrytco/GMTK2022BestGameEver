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
        if (NextTiles.Count == 0) Debug.LogWarning("Tile \"" + gameObject.name + "\" has no next tiles set!");

        PrevTiles = new List<ITile>();
        foreach (var tile in NextTiles) tile.PrevTiles.Add(this);

        DrawConnections();
    }

    private void DrawConnections()
    {
        if (NextTiles == null) return;
        foreach (var tile in NextTiles)
        {
            Vector3[] points = { transform.position, tile.transform.position };
            var linePrefab = Instantiate(LinePrefab, gameObject.transform);
            var lineRenderer = linePrefab.GetComponent<LineRenderer>();
            lineRenderer.SetPositions(points);
        }
    }

    public override bool Fight(List<ICharacter> defenders, ICharacter attacker)
    {
        throw new NotImplementedException();
    }
}