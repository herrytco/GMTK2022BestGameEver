using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Interfaces.ICharacter selectedCharacter;
    private List<Team> teams = new List<Team>();
    private int activeTeamIndex;
    private int rollResult;


    public ICharacter SelectedCharacter { get => selectedCharacter; set => selectedCharacter = value; }
    public List<Team> Teams { get => teams; set => teams = value; }
    public Team GetActiveTeam => Teams[activeTeamIndex];
    public int RollResult { get => rollResult; private set => rollResult = value; }

    // Start is called before the first frame update
    void Start()
    {
        teams.Add(new Team("test0", 0));
        teams.Add(new Team("test1", 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn()
    {
        if (++activeTeamIndex >= Teams.Count)
            activeTeamIndex = 0;
        selectedCharacter = null;
    }

    public void MovePiece(ICharacter piece)
    {
        piece.CurrentTile.Leave(piece, piece.CurrentTile);
        ITile tile = piece.CurrentTile;
        for (;rollResult > 0; rollResult--)
        {
            if(tile.NextTiles.Count > 1)
            {
                //Enable Tile Selection
                return;
            }

            tile = tile.NextTiles[0];

            tile.Visit(piece);
        }
        tile = tile.NextTiles[0];
        tile.Occupy(piece);
    }

}
