using Interfaces;
using UnityEngine;

public class PlayerController : ICharacter
{
    private GameManager gameManager;


    // Start is called before the first frame update
    private void Start()
    {
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


    public void ComfirmMovement()
    {
        ConfirmationCanvas.SetActive(false);
        gameManager.Moving = true;
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
            CurrentTile.Visit(this, gameManager.RegisterVisitCallback);
        }
        else
        {
            CurrentTile.Occupy(this, gameManager.RegisterOccupyCallback);
        }
        return;
    }

    public void DenyMovement()
    {
        ConfirmationCanvas.SetActive(false);
    }

    public override void AnimateMovement(ITile tile, float t)
    {
        
    }
}