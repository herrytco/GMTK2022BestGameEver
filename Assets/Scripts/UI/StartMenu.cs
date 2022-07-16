using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject customiseScreenHolder;
    private bool customiseScreenState = false;
    private int currentTeamSize;
    public GameObject teamSize;
    public ScrollViewLogic scrollMenu;


    private void Start()
    {
        if (customiseScreenHolder != null) customiseScreenHolder.SetActive(customiseScreenState);
    }

    public void ToggleCustomScreen()
    {
        customiseScreenState = !customiseScreenState;
        if (customiseScreenHolder != null) customiseScreenHolder.SetActive(customiseScreenState);
    }

    public void SendInputString(String manualInput)
    {
        //names of characters,should be connected to scrollable list containers
    }

    public void AddPlayersPerTeam()
    {
        currentTeamSize++;
        if (teamSize == null) return;
        Text team = teamSize.GetComponent<Text>();
        team.text= "Players per team: " + currentTeamSize;
    }

    public void RemovePlayersPerTeam()
    {
        if (currentTeamSize > 0) currentTeamSize--;
        if (teamSize == null) return;
        Text team = teamSize.GetComponent<Text>();
        team.text = "Players per team: " + currentTeamSize;
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