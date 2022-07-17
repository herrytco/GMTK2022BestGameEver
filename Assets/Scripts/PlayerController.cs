using Interfaces;
using UnityEngine;

public class PlayerController : ICharacter
{
    private GameManager gameManager;
    [SerializeField] private ITile spawn;


    // Start is called before the first frame update
    private void Start()
    {
        CurrentTile = spawn;
        transform.position = CurrentTile.transform.position;
        spawn.Occupy(this, (tile, character) => gameManager.RegisterOccupyCallback(tile, character));
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //GetComponent<SpriteRenderer>().color = Team.Color;
        ConfirmationCanvas = transform.Find("ConfirmationCanvas").gameObject;
        ConfirmationCanvas.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnMouseDown()
    {
        if (Team.Id != gameManager.GetActiveTeam.Id)
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
    public override void MoveOneStep(bool onlyVisiting, int moveToTileId = 0)
    {
        if (CurrentTile.NextTiles.Count > 1 && moveToTileId != 0)
        {
            gameManager.EnableTileSelectionUI(CurrentTile);
            return;
        }

        CurrentTile = CurrentTile.NextTiles[moveToTileId];


        if (onlyVisiting)
        {
            CurrentTile.Visit(this, (tile, character) => gameManager.RegisterVisitCallback(tile, character));
        }
        else
        {
            CurrentTile.Occupy(this, (tile, character) => gameManager.RegisterOccupyCallback(tile, character));
        }
        return;
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
            gameManager.MoveDone = true;
    }

    public override void Kill()
    {

        //Particle Effects

        

        Destroy(gameObject);
    }
}