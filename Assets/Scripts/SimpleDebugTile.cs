using System;
using Interfaces;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SimpleTile))]
public class SimpleDebugTile : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var tile = GetComponent<SimpleTile>();
        foreach (var nextTile in tile.myNextTiles)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(tile.transform.position, nextTile.transform.position);
        }
    }
}