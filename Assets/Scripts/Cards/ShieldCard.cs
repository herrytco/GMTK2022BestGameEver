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
            ICharacter selectedChar;
                
            try
            {
                selectedChar = manager.GetSelectedCharacterForCardExecution(this);
            }
            catch (NoCharacterSelectedException)
            {
                Debug.Log("No char selected, ... aborting!");
                return;
            }
            
            selectedChar.GiveShield();
            manager.TextManager.SetMessage(selectedChar.name+" ("+manager.GetActiveTeam.Name+") received a shield!");
        }
    }
}