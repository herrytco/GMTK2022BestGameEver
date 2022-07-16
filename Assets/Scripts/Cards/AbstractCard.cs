using System;
using TMPro;
using UnityEngine;

namespace Cards
{
    public abstract class AbstractCard : MonoBehaviour
    {
        private TextMeshProUGUI _costText;
        private TextMeshProUGUI _descriptionText;
        private TextMeshProUGUI _diceText;
        private TextMeshProUGUI _flavorText;
        private TextMeshProUGUI _titleText;

        private void Start()
        {
            var textboxes = transform.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (var box in textboxes)
            {
                var textName = box.name;

                if (textName.Contains("cost", StringComparison.CurrentCultureIgnoreCase))
                    _costText = box;

                if (textName.Contains("name", StringComparison.CurrentCultureIgnoreCase))
                    _titleText = box;

                if (textName.Contains("description", StringComparison.CurrentCultureIgnoreCase))
                    _descriptionText = box;

                if (textName.Contains("dice", StringComparison.CurrentCultureIgnoreCase))
                    _diceText = box;

                if (textName.Contains("flavor", StringComparison.CurrentCultureIgnoreCase))
                    _flavorText = box;
            }

            UpdateCostText(GetCardData().Costs.ToString());
            UpdateNameText(GetCardData().CardName);
            UpdateDescriptionText(GetCardData().Description);
            UpdateDiceText(GetCardData().DiceText);
            UpdateFlavorText(GetCardData().FlavorText);
        }

        public void UpdateCostText(string text)
        {
            _costText.text = text;
        }

        public void UpdateNameText(string text)
        {
            _titleText.text = text;
        }

        public void UpdateDescriptionText(string text)
        {
            _descriptionText.text = text;
        }

        public void UpdateDiceText(string text)
        {
            _diceText.text = text;
        }

        public void UpdateFlavorText(string text)
        {
            _flavorText.text = text;
        }

        public abstract void ExecuteEffect();

        public abstract CardData GetCardData();
    }
}