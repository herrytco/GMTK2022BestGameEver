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
        gameManager.MovePiece(this);
    }

    public void DenyMovement()
    {
        ConfirmationCanvas.SetActive(false);
    }
}