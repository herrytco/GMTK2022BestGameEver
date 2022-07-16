using System.Collections.Generic;
using Cards;
using Interfaces;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int activeTeamIndex = -1;
    private int _round = 1;
    
    public ICharacter SelectedCharacter { get; set; }

    public List<Team> Teams { get; set; } = new();
    public Team GetActiveTeam => Teams[activeTeamIndex];
    public int RollResult { get; private set; }

    [SerializeField] private GameObject teamCardManagerPrefab;
    [SerializeField] private GameObject endTurnButton;
    [SerializeField] private TextManager textManager;

    public TextManager TextManager
    {
        get => textManager;
    }

    private readonly Dictionary<Team, TeamCardManager> _cardManagers = new();

    private void Start()
    {
        Teams.Add(new Team("Red Hawks", 0));
        Teams.Add(new Team("Blue Giraffes", 1));

        // disable the end-turn button on startup
        endTurnButton.gameObject.SetActive(false);

        // create one card manager for each team. Deactivate them and active only the one of the active team.
        foreach (var team in Teams)
        {
            TeamCardManager mng = Instantiate(teamCardManagerPrefab).GetComponent<TeamCardManager>();
            mng.gameObject.SetActive(false);
            mng.Team = team;
            mng.GameManager = this;

            _cardManagers[team] = mng;
        }

        // start the first turn
        NextTurn();
    }

    public void NextTurn()
    {
        if (activeTeamIndex >= 0)
        {
            _cardManagers[GetActiveTeam].gameObject.SetActive(false);
        }

        if (++activeTeamIndex >= Teams.Count)
        {
            activeTeamIndex = 0;
            _round++;
        }

        textManager.SetMessage("New Turn! Let's go Team " + GetActiveTeam.Name);
        textManager.SetTurn(_round);

        SelectedCharacter = null;
        endTurnButton.SetActive(false);

        _cardManagers[GetActiveTeam].gameObject.SetActive(true);
    }

    /// <summary>
    /// Called when the player made a move
    /// </summary>
    public void PlayerUsedDice()
    {
        // activate the end turn button
        endTurnButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Called when the player used a skill
    /// </summary>
    public void PlayerUsedSkills()
    {
        // activate the end turn button
        endTurnButton.gameObject.SetActive(true);
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

        PlayerUsedDice();
    }
}