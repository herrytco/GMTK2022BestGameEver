using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TextManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private TextMeshProUGUI messageText;

        public void SetTurn(int turn)
        {
            turnText.text = "Turn " + turn;
        }

        public void SetMessage(string message)
        {
            messageText.text = message;
        }
    }
}