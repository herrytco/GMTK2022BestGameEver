using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Interfaces;
using UnityEngine;

public class CharSelectionAcceptButton : MonoBehaviour
{
    [SerializeField] private PieceController character;
    
    private void OnMouseUp()
    {
        Debug.Log("nice!");
        
        character.GameManager.SaveCharacterSelection();
    }
}
