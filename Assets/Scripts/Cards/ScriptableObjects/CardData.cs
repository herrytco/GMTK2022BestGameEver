using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "New Card Data", menuName = "ScriptableObjects/Create Generic Card Type", order = 1)]
    public class CardData : ScriptableObject
    {
        [SerializeField] private string cardName;
        [SerializeField] private int costs;
        [SerializeField] private string description;
        [SerializeField] private string flavorText;
        [SerializeField] private string diceText;
        
        public string CardName => cardName;
        public int Costs => costs;
        public string Description => description;
        public string FlavorText => flavorText;
        public string DiceText => diceText;
    }
}


