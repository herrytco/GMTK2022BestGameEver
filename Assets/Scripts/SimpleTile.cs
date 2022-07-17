using System.Collections;
using System.Collections.Generic;
using Events;
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

    public ICharacter TestChar;

    public void TestEvent()
    {
        if (Characters.Count == 0) Characters.Add(TestChar);
        StartCoroutine(TestRoutine());
    }

    private bool _testWaiting;

    public IEnumerator TestRoutine()
    {
        var gm = gameObject.AddComponent<GameManager>();
        gm.Teams = new List<Team> { new Team("", 1) };
        gm.SetActiveTeam(0);

        _testWaiting = true;
        Occupy(gm, TestChar, (_, _) => _testWaiting = false);
        yield return new WaitWhile(() => _testWaiting);

        print("occupied, waiting");
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 4; i++)
        {
            _testWaiting = true;
            TurnProgress(gm, 0, false, (_) => _testWaiting = false);
            yield return new WaitWhile(() => _testWaiting);

            print("turn completed, waiting");
            yield return new WaitForSeconds(1f);
        }
    }
}