using System.Collections.Generic;
using System.Linq;
using Cards;
using Interfaces;
using UI;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int activeTeamIndex = -1;
    private int _round = 1;

    private bool animatingMovement;
    private bool moveAnimationDone;
    private bool waitForEvents;
    private float moveAnimTimer;

    public int ActiveTeamMana { get; set; }
    public int TargetTileID { private get; set; } = -1;
    private List<GameObject> movementSelectionUI = new();
    
    private static readonly System.Random Rng = new();

    [SerializeField] private GameObject MovementSelectionGO;
    [SerializeField] private float moveAnimSpeed;
    [SerializeField] private int piecesPerTeam = 1;

    public ICharacter SelectedCharacter { get; set; }

    public List<Team> Teams { get; set; } = new();
    public Team GetActiveTeam => Teams[activeTeamIndex];
    public int RollResult { get; private set; } = 6;

    public bool AnimatingMovement
    {
        get => animatingMovement;
        set => animatingMovement = value;
    }

    [SerializeField] private TeamCardManager teamCardManagerPrefab;
    [SerializeField] private GameObject endTurnButton;
    [SerializeField] private TextManager textManager;
    [SerializeField] private GameObject rollSkillDecisionCanvas;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PieceController piecePrefab;
    [SerializeField] private GameObject mapHolder;

    private bool _inCharSelectionMode = false;
    private AbstractCard _cardAwaitingToBeExecuted;

    public TextManager TextManager
    {
        get => textManager;
    }

    public bool MoveAnimDone
    {
        get => moveAnimationDone;
        set => moveAnimationDone = value;
    }

    public bool WaitForEvents
    {
        get => waitForEvents;
        protected set => waitForEvents = value;
    }

    private readonly Dictionary<Team, TeamCardManager> _cardManagers = new();
    private readonly Dictionary<Team, List<PieceController>> _teamPieces = new();

    private SimpleTile[] _mapTiles;

    private void Start()
    {
        Teams.Add(new Team("Red Hawks", 0));
        Teams.Add(new Team("Blue Giraffes", 1));

        _mapTiles = mapHolder.GetComponentsInChildren<SimpleTile>();

        List<SimpleTile> possibleSpawnTiles = _mapTiles.ToList();
        possibleSpawnTiles = possibleSpawnTiles.OrderBy(a => Rng.Next()).ToList();
        

        Debug.Log("Map has " + _mapTiles.Length + " tiles");

        // disable the end-turn button on startup
        endTurnButton.gameObject.SetActive(false);

        // create one card manager for each team. Deactivate them and active only the one of the active team.
        foreach (var team in Teams)
        {
            // spawn a deck/card-system for each team
            TeamCardManager mng = Instantiate(teamCardManagerPrefab);
            mng.gameObject.SetActive(false);
            mng.Team = team;
            mng.GameManager = this;
            mng.DecisionCanvas = rollSkillDecisionCanvas;

            _cardManagers[team] = mng;

            // spawn the players (on random tiles #fixme)
            List<PieceController> teamPieces = new();

            for (int i = 0; i < piecesPerTeam; i++)
            {
                PieceController piece = Instantiate(piecePrefab, mapHolder.transform);

                SimpleTile spawnTile = possibleSpawnTiles[0];
                possibleSpawnTiles.RemoveAt(0);

                piece.Spawn = spawnTile;
                piece.SpawnPiece();

                piece.name = "Piece "+(i+1)+" ("+team.Name+")";
                
                teamPieces.Add(piece);
            }

            _teamPieces[team] = teamPieces;
        }


        // start the first turn
        NextTurn();
    }

    private void Update()
    {
        if (RollResult > 0 && AnimatingMovement && !WaitForEvents)
        {
            if (TargetTileID == -1 && SelectedCharacter.CheckForCrossroads())
            {
                EnableTileSelectionUI(SelectedCharacter.CurrentTile);
                AnimatingMovement = false;
                moveAnimTimer = 0;
                return;
            }

            if (TargetTileID == -1)
                TargetTileID++;

            moveAnimTimer += Time.deltaTime * moveAnimSpeed;
            SelectedCharacter.AnimateMovement(SelectedCharacter.CurrentTile.NextTiles[TargetTileID], moveAnimTimer);
        }

        if (RollResult > 0 && MoveAnimDone && !WaitForEvents)
        {
            //Disable movement anim
            SelectedCharacter.PieceAnimator.SetBool("moving", false);
            AnimatingMovement = false;
            moveAnimTimer = 0;
            MoveAnimDone = false;
            waitForEvents = true;
            SelectedCharacter.MoveOneStep(RollResult >= 1 ? true : false, TargetTileID);


            RollResult--;

            TargetTileID = -1;
        }
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
        _cardManagers[GetActiveTeam].IsCardDrawEnabled = true;

        ActiveTeamMana = GetActiveTeam.ManaCapacity;
    }

    /// <summary>
    /// Called when the player made a move
    /// </summary>
    public void PlayerUsedDice(int result)
    {
        RollResult = result;

        // activate the end turn button
        endTurnButton.gameObject.SetActive(true);

        TextManager.SetMessage("Team " + GetActiveTeam.Name + " rolled a " + RollResult + "!");
    }

    /// <summary>
    /// Called when the player used a skill
    /// </summary>
    public void PlayerUsedSkills(AbstractCard card)
    {
        TextManager.SetMessage(GetActiveTeam.Name + " used " + card.GetCardData().name + "!");

        // activate the end turn button
        endTurnButton.gameObject.SetActive(true);
    }

    public void EnableTileSelectionUI(ITile tile)
    {
        foreach (ITile nextTile in tile.NextTiles)
        {
            Vector2 dirVec = nextTile.transform.position - tile.transform.position;
            Vector2 dirUnitVec = dirVec / dirVec.magnitude;
            GameObject tmp = Instantiate(MovementSelectionGO, dirUnitVec * dirVec.magnitude, Quaternion.identity);
            tmp.GetComponent<MovementSelectionButton>().TileId = tile.NextTiles.IndexOf(nextTile);
            tmp.GetComponent<Canvas>().worldCamera = Camera.main;
            movementSelectionUI.Add(tmp);
        }
    }

    public void DisableTileSelectionUi()
    {
        foreach (GameObject button in movementSelectionUI)
        {
            button.GetComponent<MovementSelectionButton>().Kill();
        }
    }

    public ICharacter GetSelectedCharacterForCardExecution(AbstractCard card)
    {
        if (SelectedCharacter == null)
        {
            _cardAwaitingToBeExecuted = card;
            EnableCharSelectionUI();
            throw new NoCharacterSelectedException();
        }

        return SelectedCharacter;
    }

    private void EnableCharSelectionUI()
    {
        _inCharSelectionMode = true;
        // cameraController.JumpToPiece();
    }

    public void RegisterVisitCallback(ITile tile, ICharacter piece)
    {
        WaitForEvents = false;
        AnimatingMovement = true;
    }

    public void RegisterOccupyCallback(ITile tile, ICharacter piece)
    {
        waitForEvents = false;
        AnimatingMovement = false;
        moveAnimTimer = 0;
        SelectedCharacter.MustLeave = true;
    }

    public void RegisterLeaveCallback(ITile tile)
    {
        SelectedCharacter.MustLeave = false;
    }

    public void GiveActiveTeamMana()
    {
        GetActiveTeam.ManaCapacity++;
        ActiveTeamMana++;
    }

    /// <summary>
    /// Uses mana of the active team.
    /// </summary>
    /// <param name="amount">The amount of mana.</param>
    /// <returns>true if mana was consumed. false if amount exceeded the available mana</returns>
    public bool UseMana(int amount)
    {
        int tmpMana = ActiveTeamMana - amount;

        if (tmpMana < 0)
        {
            return false;
        }

        ActiveTeamMana = tmpMana;
        return true;
    }

    /// <summary>
    /// PERMANENTLY REMOVES MANA FROM ONE TEAM
    /// </summary>
    /// <param name="team">the team that will be fucked</param>
    /// <param name="amount">the size of the fuckery</param>
    public void RemoveManaFromTeam(Team team, int amount)
    {
        team.ManaCapacity -= amount;
        if (team.ManaCapacity < 0)
            team.ManaCapacity = 0;

    }
}