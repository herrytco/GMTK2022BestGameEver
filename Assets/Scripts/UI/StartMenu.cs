using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject customiseScreenHolder;
    private bool customiseScreenState = false;
    private int startTeamSize = 1;
    private int currentTeamSize;
    public TextMeshProUGUI teamSizeText;
    public GameObject scrollableContainerPrefab;
    
    private void Start()
    {
        if (customiseScreenHolder != null) customiseScreenHolder.SetActive(customiseScreenState);
        if (teamSizeText == null) return;
        currentTeamSize = startTeamSize;
        teamSizeText.SetText("Players per team: " + currentTeamSize);
    }

    /// <summary>
    /// Shows and hides customisation menu
    /// </summary>
    public void ToggleCustomScreen()
    {
        customiseScreenState = !customiseScreenState;
        if (customiseScreenHolder != null) customiseScreenHolder.SetActive(customiseScreenState);
    }

    
    public void SendInputString(String manualInput)
    {
        //names of characters,should be connected to scrollable list containers
    }
    
    /// <summary>
    /// Adds one additional default player character to all team
    /// </summary>
    public void AddPlayersPerTeam()
    {
        currentTeamSize++;
        if (teamSizeText == null) return;
        teamSizeText.SetText("Players per team: " + currentTeamSize);
        
        if(scrollableContainerPrefab == null) return;
        GameObject container = Instantiate(scrollableContainerPrefab, transform);
    }

    /// <summary>
    /// Subtracts last player character from current team
    /// </summary>
    public void RemovePlayersPerTeam()
    {
        if (currentTeamSize > 1) currentTeamSize--;
        if (teamSizeText == null) return;
        teamSizeText.SetText("Players per team: " + currentTeamSize);
    }

    public void SaveAllChanges()
    {
        print(currentTeamSize);
    }

    /// <summary>
    /// Sets Player Characters per Team
    /// </summary>
    public void SetPlayerPerTeam(int playerPerTeam, GameObject dropdownMenu)
    {
        if (dropdownMenu == null) return;
        Dropdown currentDropdown = dropdownMenu.GetComponent<Dropdown>();
        currentDropdown.ClearOptions();

        for (int i = 0; i < playerPerTeam; i++)
        {
            var newDeafaultPlayer = new Dropdown.OptionData();
            newDeafaultPlayer.text = "Player " + i;
            currentDropdown.options.Add(newDeafaultPlayer);
        }
    }
}