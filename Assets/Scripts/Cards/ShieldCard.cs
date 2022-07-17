using Interfaces;
using UnityEngine;

namespace Cards
{
    public class ShieldCard : AbstractCard
    {
        [SerializeField] private ShieldCardData data;

        public override CardData GetCardData()
        {
            return data;
        }

        public override void ExecuteEffect(GameManager manager)
        {
            try
            {
                ICharacter selectedChar = manager.GetSelectedCharacterForCardExecution(this);
            }
            catch (NoCharacterSelectedException)
            {
                Debug.Log("No char selected, ... aborting!");
            }
        }
    }
}