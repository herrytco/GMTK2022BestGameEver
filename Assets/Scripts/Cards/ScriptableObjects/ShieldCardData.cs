using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "New Card Data", menuName = "ScriptableObjects/Create Shield Card Type", order = 2)]
    public class ShieldCardData : CardData
    {
        [SerializeField] private string durationDice;

        public string DurationDice => durationDice;
    }
}