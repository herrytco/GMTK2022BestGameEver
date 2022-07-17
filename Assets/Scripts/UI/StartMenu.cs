using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject settingsScreen; //The pop-up menu with all the settings
    private bool settingsScreenActive = false;

    private int startTeamSize = 1;
    private int currentTeamSize;
    public TMP_Dropdown teamDropdown;
    private List<TMP_Dropdown.OptionData> currentTeamOptions = new List<TMP_Dropdown.OptionData>();

    private int numTeamsStart = 2;
    private int currentTeamSelected;
    public TextMeshProUGUI teamSizeText;

    public GameObject scrollableContainerPrefab;
    public GameObject containerParent;

    private void Start()
    {
        if (settingsScreen != null) settingsScreen.SetActive(settingsScreenActive);
        if (teamSizeText == null) return;
        currentTeamSize = startTeamSize;
        teamSizeText.SetText("Players per team: " + currentTeamSize);
        UpdateUI();
    }

    void UpdateUI()
    {
        List<Team> currentTeams = GameData.Instance._teams;
        teamDropdown.options.Clear();

        if (currentTeams.Count <= 0) return;
        foreach (var team in currentTeams)
        {
            TMP_Dropdown.OptionData t = new TMP_Dropdown.OptionData(team.Name);
            currentTeamOptions.Add(t);
            teamDropdown.options = currentTeamOptions;
        }

        DrawPlayerList(currentTeams[teamDropdown.value]);

        if (teamSizeText == null) return;
        teamSizeText.SetText("Players per team: " + currentTeams[0].characterNames.Count);
    }

    /// <summary>
    /// Shows and hides customisation menu
    /// </summary>
    public void ToggleCustomScreen()
    {
        settingsScreenActive = !settingsScreenActive;
        if (settingsScreen != null) settingsScreen.SetActive(settingsScreenActive);
    }

    /// <summary>
    /// Populates scroll menu
    /// </summary>
    void DrawPlayerList(Team selectedTeam)
    {
        foreach (Transform child in containerParent.transform) {
            Destroy(child.gameObject);
        }
        
        if (scrollableContainerPrefab == null || containerParent == null) return;
        List<Team> currentTeams = GameData.Instance._teams;
        foreach (var name in currentTeams[teamDropdown.value].characterNames)
        {
            GameObject container = Instantiate(scrollableContainerPrefab, containerParent.transform);
            //get container script
            //edit name
        }

    }

    public void AddCharacters()
    {
        GameData.Instance.AddPlayersPerTeam();
        UpdateUI();
    }

    public void AddTeam()
    {
        GameData.Instance.AddTeam();
        UpdateUI();
    }

    public void RemoveCharacters()
    {
        GameData.Instance.RemovePlayersPerTeam();
        UpdateUI();
    }

    public void RemoveTeam()
    {
        if (teamDropdown == null) return;
        GameData.Instance.RemoveTeam(teamDropdown.value);
        UpdateUI();
    }

    public void SaveAllChanges()
    {
        //send data to game data
    }
}