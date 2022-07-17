using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DataContainer : MonoBehaviour
    {
        public String characterName;
        public String inputName;
        public TextMeshProUGUI characterNameText;
        public TMP_InputField inputField;
        private bool inputFieldActive;

        private void Start()
        {
            inputFieldActive = false;
            inputField.gameObject.SetActive(inputFieldActive);
        }

        public void ChangeName(String newName)
        {
            characterName = newName;
            characterNameText.SetText(newName);
        }

        /// <summary>
        /// Change banner name
        /// </summary>
        public void OnTextInput()
        {
            inputName = inputField.text;
        }

        /// <summary>
        /// Activate textbox and deactivate name banner
        /// </summary>
        public void OnEdit()
        {
            inputFieldActive = !inputFieldActive;
            inputField.transform.gameObject.SetActive(inputFieldActive);
            characterNameText.gameObject.SetActive(!inputFieldActive);
            ChangeName(inputName);
        }
        
    }
}