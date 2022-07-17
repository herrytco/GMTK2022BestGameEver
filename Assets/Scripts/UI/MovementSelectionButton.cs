using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSelectionButton : MonoBehaviour
{

    private GameManager gameManager;
    public int TileId { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        OnPress();
    }

    public void OnPress()
    {
        Debug.Log(TileId);
        gameManager.TargetTileID = TileId;
        gameManager.AnimatingMovement = true;
        gameManager.DisableTileSelectionUi();
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        DestroyImmediate(gameObject);
    }
}
