using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    public class GameData
    {
        #region Singleton

        private static GameData _instance;

        public static GameData Instance
        {
            get
            {
                if (_instance == null) _instance = new GameData();
                return _instance;
            }
        }

        private GameData()
        {
        }

        #endregion


        public List<Team> _teams = new List<Team>();
        private int currentTeamSize = 1;

        private String[] randomCharacterNames = new[]
        {
            "Jack",
            "Hans",
            "Martin",
            "Marion",
            "Mizi",
            "Megadolon",
            "Moritz",
            "Aidus",
            "Achillus",
            "Orangina",
            "Stabcutter",
            "Metall Boi",
            "Hermes",
            "Herrmann",
            "Wisolak",
            "V",
            "CJ",
            "Mister Woodpecker",
            "Floppy",
            "Franziskus",
            "Merinelda",
            "Xipho",
            "Randel"
        };

        List<String> GenerateCharacters(int numCharacters)
        {
            List<String> characters = new List<String>();
            for (int i = 0; i < numCharacters; i++)
            {
                var name = RandomName();
                if (characters.Contains(name)) name = RandomName();
                characters.Add(name);
            }

            return characters;
        }

        String RandomName()
        {
            Random rnd = new Random();
            int num = rnd.Next(0, randomCharacterNames.Length);
            return randomCharacterNames[num];
        }

        /// <summary>
        /// Adds one Team
        /// </summary>
        public void AddTeam()
        {
            var name = "Default team: " + _teams.Count;
            var defTeam = new Team(name, _teams.Count);
            List<String> characters = GenerateCharacters(currentTeamSize);
            defTeam.characterNames = characters;
            _teams.Add(defTeam);
        }

        /// <summary>
        /// Subtracts selected team
        /// </summary>
        public void RemoveTeam(int currentTeam)
        {
            if (_teams == null || _teams.Count<=0) return;
            _teams.RemoveAt(currentTeam);
        }


        /// <summary>
        /// Adds one additional default player character to all teams
        /// </summary>
        public void AddPlayersPerTeam()
        {
            currentTeamSize++;
            if (_teams == null || _teams.Count <= 0) return;
            foreach (var team in _teams)
            {
                team.characterNames.Add(RandomName());
            }
        }

        /// <summary>
        /// Subtracts last player character from all teams
        /// </summary>
        public void RemovePlayersPerTeam()
        {
            if (currentTeamSize <= 1) return;
            
            currentTeamSize--;
            if (_teams == null || _teams.Count <= 0) return;
            foreach (var team in _teams)
            {
                team.characterNames.RemoveAt(team.characterNames.Count - 1);
            }
        }
    }
}