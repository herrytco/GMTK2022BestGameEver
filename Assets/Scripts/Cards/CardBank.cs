using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class CardBank : MonoBehaviour
    {
        [SerializeField] private List<AbstractCard> handCards = new();
        [SerializeField] private float outerPadding = 10f;

        private float _screenHeight;
        private float _screenWidth;

        private void Start()
        {
            _screenHeight = ScreenHeight;
            _screenWidth = ScreenWidth;
        }

        private void Update()
        {
            if (_screenHeight != ScreenHeight || _screenWidth != ScreenWidth)
                RedrawCards();
        }

        public void AddCard(AbstractCard card)
        {
            handCards.Add(Instantiate(card, transform));
            RedrawCards();
        }

        public void RedrawCards()
        {
            for (var i = 0; i < handCards.Count; i++)
            {
                var handCard = handCards[i];

                var cardSize = handCard.GetComponentInChildren<Renderer>().bounds.size;

                var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
                bottomLeft.z = 0;
                bottomLeft.y += cardSize.y / 2;

                var bottomRight = bottomLeft + new Vector3(ScreenWidth, 0, 0);

                var bottomLeftPadded = bottomLeft + Vector3.right * outerPadding;
                var bottomRightPadded = bottomRight + Vector3.left * outerPadding;

                var bottomLeftCardSpace = bottomLeftPadded + Vector3.right * cardSize.x / 2;
                var bottomRightCardSpace = bottomRightPadded + Vector3.left * cardSize.x / 2;

                float widthSegment = (bottomRightCardSpace.x - bottomLeftCardSpace.x) / (handCards.Count + 1);

                handCard.transform.position = bottomLeftCardSpace + Vector3.right * (widthSegment * (i + 1));
                handCard.AdjustOrderIndex(i * handCards.Count);
            }
        }

        private float ScreenHeight => Camera.main.orthographicSize * 2;

        private float ScreenWidth => ScreenHeight * (Screen.width / (float)Screen.height);
    }
}