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
            if (nextTile == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(tile.transform.position, nextTile.transform.position);
        }
    }
}