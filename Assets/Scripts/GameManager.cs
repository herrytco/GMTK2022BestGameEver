using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int activeTeamIndex;


    public ICharacter SelectedCharacter { get; set; }

    public List<Team> Teams { get; set; } = new();
    public Team GetActiveTeam => Teams[activeTeamIndex];
    public int RollResult { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        Teams.Add(new Team("test0", 0));
        Teams.Add(new Team("test1", 1));
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void NextTurn()
    {
        if (++activeTeamIndex >= Teams.Count)
            activeTeamIndex = 0;
        SelectedCharacter = null;
    }

    public void MovePiece(ICharacter piece)
    {
        piece.CurrentTile.Leave(piece, piece.CurrentTile, (tile, piece) => { });
        var tile = piece.CurrentTile;
        for (; RollResult > 0; RollResult--)
        {
            if (tile.NextTiles.Count > 1)
                //Enable Tile Selection
                return;

            tile = tile.NextTiles[0];

            tile.Visit(piece, (tile, piece) => { });
        }

        tile = tile.NextTiles[0];
        tile.Occupy(piece, (tile, piece) => { });
    }
}