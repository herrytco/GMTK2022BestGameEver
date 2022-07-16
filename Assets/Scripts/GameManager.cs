using System.Collections.Generic;
using Cards;
using Interfaces;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int activeTeamIndex = -1;
    private int _round = 1;

    private bool animatingMovement;
    private bool moveAnimationDone;
    private bool waitForEvents;
    private float moveAnimTimer;
    public int TargetTileID { private get; set; }
    private List<GameObject> movementSelectionUI = new();
    
    [SerializeField] private GameObject MovementSelectionGO;
    [SerializeField] private float moveAnimSpeed;

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

    private void Start()
    {
        Teams.Add(new Team("Red Hawks", 0));
        Teams.Add(new Team("Blue Giraffes", 1));

        // disable the end-turn button on startup
        endTurnButton.gameObject.SetActive(false);

        // create one card manager for each team. Deactivate them and active only the one of the active team.
        foreach (var team in Teams)
        {
            TeamCardManager mng = Instantiate(teamCardManagerPrefab);
            mng.gameObject.SetActive(false);
            mng.Team = team;
            mng.GameManager = this;
            mng.DecisionCanvas = rollSkillDecisionCanvas;

            _cardManagers[team] = mng;
        }

        // start the first turn
        NextTurn();
    }

    private void Update()
    {
        if (RollResult > 0 && animatingMovement && !WaitForEvents)
        {
            if (SelectedCharacter.CheckForCrossroads())
            {
                animatingMovement = false;
                return;
            }

            moveAnimTimer += Time.deltaTime * moveAnimSpeed;
            SelectedCharacter.AnimateMovement(SelectedCharacter.CurrentTile.NextTiles[TargetTileID], moveAnimTimer);
        }

        if (RollResult > 0 && MoveAnimDone && !WaitForEvents)
        {
            //Disable movement anim
            animatingMovement = false;
            moveAnimationDone = false;
            SelectedCharacter.MoveOneStep(RollResult >= 1 ? true : false, TargetTileID);
            waitForEvents = true;
            
            RollResult--;

            TargetTileID = 0;
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
    }

    /// <summary>
    /// Called when the player made a move
    /// </summary>
    public void PlayerUsedDice(int result)
    {
        RollResult = result;
        
        // activate the end turn button
        endTurnButton.gameObject.SetActive(true);
        
        TextManager.SetMessage("Team "+GetActiveTeam.Name+" rolled a "+RollResult+"!");
    }

    /// <summary>
    /// Called when the player used a skill
    /// </summary>
    public void PlayerUsedSkills()
    {
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

            movementSelectionUI.Add(tmp);
            tmp.GetComponent<MovementSelectionButton>().TileId = tile.NextTiles.IndexOf(nextTile);
        }
    }

    public void DisableTileSelectionUi()
    {
        foreach (GameObject button in movementSelectionUI)
        {
            Destroy(button);
        }

        movementSelectionUI = new();
    }

    public void RegisterVisitCallback(ITile tile, ICharacter piece)
    {
        Debug.Log("e");
        WaitForEvents = false;
        animatingMovement = true;
    }

    public void RegisterOccupyCallback(ITile tile, ICharacter piece)
    {
        waitForEvents = false;
    }

    public void RegisterLeaveCallback(ITile tile)
    {
        SelectedCharacter.MustLeave = false;
    }
}