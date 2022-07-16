using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

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

        // initialize effects from children
        foreach (var obs in GetComponentsInChildren<IEventObserver<TileEvent>>())
            AddObserver(obs);

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
}