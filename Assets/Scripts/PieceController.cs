using Interfaces;
using UnityEngine;

public class PieceController : ICharacter
{
    private GameManager gameManager;
    [SerializeField] private ITile spawn;



    // Start is called before the first frame update
    private void Start()
    {
        CurrentTile = spawn;
        transform.position = CurrentTile.transform.position;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //GetComponent<SpriteRenderer>().color = Team.Color;
        ConfirmationCanvas = transform.Find("ConfirmationCanvas").gameObject;
        ConfirmationCanvas.SetActive(false);
        gameManager.SelectedCharacter = this;
        CurrentTile.Occupy(this, (ITile tile, ICharacter character) => gameManager.RegisterOccupyCallback(tile, character));
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnMouseDown()
    {
        Debug.Log("RollResult: " + gameManager.RollResult + "\nWaitforevents: " + gameManager.WaitForEvents);
        if (Team.Id != gameManager.GetActiveTeam.Id || gameManager.WaitForEvents || gameManager.RollResult <= 0)
            return; // maybe implement an animation for selecting piece of different team

        if (gameManager.SelectedCharacter != null)
            gameManager.SelectedCharacter.ConfirmationCanvas.SetActive(false);

        gameManager.SelectedCharacter = this;
        ConfirmationCanvas.SetActive(true);
    }

    /// <summary>
    /// Visits or Occupies next Tile based on onlyVisiting
    /// </summary>
    /// <param name="onlyVisiting">should be false if this is the last move</param>
    /// <param name="moveToTileId">the id of the tile to move to if there is more than one available</param>
    /// <returns>The tile that is currently occupied or visited</returns>
    public override void MoveOneStep(bool onlyVisiting, int moveToTileId = -1)
    {
        if (MustLeave)
        {
            MustLeave = false;
           CurrentTile.Leave(this, CurrentTile, (ITile tile, ICharacter character) => gameManager.RegisterLeaveCallback(tile));
        }

        if (moveToTileId == -1)
            moveToTileId++;

        Debug.Log("a!");

        CurrentTile = CurrentTile.NextTiles[moveToTileId];


        if (onlyVisiting)
        {

            Debug.Log("SHEESH");
            CurrentTile.Visit(this, (tile, character) => gameManager.RegisterVisitCallback(tile, character));
        }
        else
        {

            Debug.Log("oi");
            CurrentTile.Occupy(this, (tile, character) => gameManager.RegisterOccupyCallback(tile, character));
        }
        return;
    }

    /// <summary>
    /// Checks if there is more than one next tile
    /// </summary>
    /// <returns>true if there is more than one next tile</returns>
    public override bool CheckForCrossroads()
    {
        if (CurrentTile.NextTiles.Count > 1)
        {
            return true;
        }

        return false;
    }
    public void ComfirmMovement()
    {
        ConfirmationCanvas.SetActive(false);
        gameManager.AnimatingMovement = true;
    }

    public void DenyMovement()
    {
        ConfirmationCanvas.SetActive(false);
    }

    public override void AnimateMovement(ITile tile, float t)
    {
        //Start Animation
        //Move Sprite
        if (t >= 1)
            t = 1;

        transform.position = Vector2.Lerp(transform.position, tile.transform.position, t);

        //Stop Animation
        if (t == 1)
        {
            gameManager.AnimatingMovement = false;
            gameManager.MoveAnimDone = true;
        }
    }

    public override void Kill()
    {

        //Particle Effects

        

        Destroy(this);
    }
}