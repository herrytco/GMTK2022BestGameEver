using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int activeTeamIndex;
    private bool moving;
    private bool moveDone;
    private float moveAnimTimer;
    private List<GameObject> movementSelectionUI;
    [SerializeField] private GameObject MovementSelectionGO;
    [SerializeField] private float moveAnimSpeed;



    public ICharacter SelectedCharacter { get; set; }

    public List<Team> Teams { get; set; } = new();
    public Team GetActiveTeam => Teams[activeTeamIndex];
    public int RollResult { get; private set; }
    public bool Moving { get => moving; set => moving = value; }
    public int selectedTileId { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        Teams.Add(new Team("test0", 0));
        Teams.Add(new Team("test1", 1));
    }

    // Update is called once per frame
    private void Update()
    {
        if (moving)
        {
            moveAnimTimer += Time.deltaTime * moveAnimSpeed;
            //SelectedCharacter.AnimateMovement(moveAnimTimer > 1 ? 1 :  moveAnimTimer);
        }
    }

    public void NextTurn()
    {
        if (++activeTeamIndex >= Teams.Count)
            activeTeamIndex = 0;
        SelectedCharacter = null;
    }



    

    public void EnableTileSelectionUI(ITile tile)
    {

        foreach ( ITile nextTile in tile.NextTiles)
        {
            Vector2 dirVec = nextTile.transform.position - tile.transform.position;
            Vector2 dirUnitVec = dirVec / dirVec.magnitude;

            movementSelectionUI.Add(Instantiate(MovementSelectionGO, dirUnitVec*dirVec.magnitude, Quaternion.identity));
            MovementSelectionGO.GetComponent<MovementSelectionButton>().TileId = tile.NextTiles.IndexOf(nextTile);
        }
    }

    public void DisableTileSelectionUI()
    {

    }

    public void RegisterVisitCallback(ITile tile, ICharacter piece)
    {

    }

    public void RegisterOccupyCallback(ITile tile, ICharacter piece)
    {
        NextTurn();
    }
}