using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class StartMenu : MonoBehaviour
{
    public AudioClip saveAudioClip;
    public AudioClip UISelectAudioClip;
    AudioSource audioSource;
    
    public GameObject settingsScreen; //The pop-up menu with all the settings
    private bool settingsScreenActive = false;

    private int startTeamSize = 1;
    private int currentTeamSize;
    public TMP_Dropdown teamDropdown;
    private List<TMP_Dropdown.OptionData> currentTeamOptions = new List<TMP_Dropdown.OptionData>();

    private int numTeamsStart = 2;
    private int currentTeamSelected;
    public TextMeshProUGUI teamSizeText;

    public Toggle AItoggle;

    public GameObject scrollableContainerPrefab;
    public GameObject containerParent;

    public TMP_Dropdown teamColorDropdown;
    public Color[] possibleColors;

    private void Start()
    {
        if (settingsScreen != null) settingsScreen.SetActive(settingsScreenActive);
        if (teamSizeText == null) return;
        currentTeamSize = startTeamSize;
        teamSizeText.SetText("Players per team: " + currentTeamSize);

        for (int i = 0; i < numTeamsStart; i++)
        {
            GameData.Instance.AddTeam();
        }

        if (AItoggle != null) AItoggle.isOn = false;
        audioSource = GetComponent<AudioSource>();
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

        DrawPlayerList(currentTeams[CheckCurrentTeamIndex()]);

        if (teamSizeText == null) return;
        teamSizeText.SetText("Players per team: " + currentTeams[0].characterNames.Count);
    }

    public void PlaySelectSound()
    {
        if (UISelectAudioClip != null && audioSource != null) audioSource.PlayOneShot(UISelectAudioClip);
    }

    int CheckCurrentTeamIndex()
    {
        List<Team> currentTeams = GameData.Instance._teams;
        currentTeamSelected =
            (teamDropdown.value > currentTeams.Count - 1) ? currentTeams.Count - 1 : teamDropdown.value;
        return currentTeamSelected;
    }

    /// <summary>
    /// Shows and hides customisation menu
    /// </summary>
    public void ToggleCustomScreen()
    {
        if (UISelectAudioClip != null) audioSource.PlayOneShot(UISelectAudioClip);
        settingsScreenActive = !settingsScreenActive;
        if (settingsScreen != null) settingsScreen.SetActive(settingsScreenActive);
        UpdateUI();
    }

    public void OnTeamSelectChange()
    {
        UpdateUI();
        if (UISelectAudioClip != null) audioSource.PlayOneShot(UISelectAudioClip);
        if (AItoggle == null) return;
        AItoggle.isOn = false;
    }

    /// <summary>
    /// Populates scroll menu
    /// </summary>
    void DrawPlayerList(Team selectedTeam)
    {
        foreach (Transform child in containerParent.transform)
        {
            Destroy(child.gameObject);
        }

        if (scrollableContainerPrefab == null || containerParent == null) return;
        List<Team> currentTeams = GameData.Instance._teams;
        foreach (var name in currentTeams[CheckCurrentTeamIndex()].characterNames)
        {
            GameObject container = Instantiate(scrollableContainerPrefab, containerParent.transform);
            DataContainer data = container.GetComponent<DataContainer>();
            data.ChangeName(name);
        }
    }

    public void AddCharacters()
    {
        GameData.Instance.AddPlayersPerTeam();
        if (UISelectAudioClip != null) audioSource.PlayOneShot(UISelectAudioClip);
        UpdateUI();
    }

    public void AddTeam()
    {
        GameData.Instance.AddTeam();
        if (UISelectAudioClip != null) audioSource.PlayOneShot(UISelectAudioClip);
        UpdateUI();
    }

    public void RemoveCharacters()
    {
        GameData.Instance.RemovePlayersPerTeam();
        if (UISelectAudioClip != null) audioSource.PlayOneShot(UISelectAudioClip);
        UpdateUI();
    }

    public void RemoveTeam()
    {
        if (teamDropdown == null) return;
        GameData.Instance.RemoveTeam(CheckCurrentTeamIndex());
        if (UISelectAudioClip != null) audioSource.PlayOneShot(UISelectAudioClip);
        UpdateUI();
    }

    public void SaveAllChanges()
    {
        List<Team> currentTeams = GameData.Instance._teams;
        if (currentTeams == null || currentTeams.Count <= 0) return;
        Team currentTeam = currentTeams[CheckCurrentTeamIndex()];
        List<String> oldNames = currentTeam.characterNames;
        List<String> newNames = new List<string>();

        int i = 0;
        foreach (Transform child in containerParent.transform)
        {
            DataContainer data = child.gameObject.GetComponent<DataContainer>();
            var newName = (oldNames[i].Equals(data.characterName)) ? oldNames[i] : data.characterName;
            newNames.Add(newName);
            i++;
        }

        currentTeam.characterNames = newNames;
        currentTeam.Color = Color.black; //from dropdown
        currentTeam.isAi = AItoggle.isOn; //from toggle

        if (saveAudioClip != null) audioSource.PlayOneShot(saveAudioClip);
        GameData.Instance.SaveChangesCurrentTeam(CheckCurrentTeamIndex(), currentTeam);
    }
}