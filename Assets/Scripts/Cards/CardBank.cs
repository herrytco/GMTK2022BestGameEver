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

        private TeamCardManager _teamCardManager;
        private GameObject _decisionCanvas;

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
            AbstractCard cardNew = Instantiate(card, transform);
            cardNew.Bank = this;
            cardNew.Usable = true;

            handCards.Add(cardNew);
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

                handCard.PositionInCardBank = bottomLeftCardSpace + Vector3.right * (widthSegment * (i + 1));

                handCard.AdjustOrderIndex(i * handCards.Count);
            }
        }

        public void UseCardDice(AbstractCard card)
        {
            RemoveCard(card);
            _teamCardManager.ReportDiceRoll(card);
        }

        public void UseCardSkill(AbstractCard card)
        {
            RemoveCard(card);
            _teamCardManager.ReportSkillUse(card);
        }

        private void RemoveCard(AbstractCard card)
        {
            handCards.Remove(card);
            RedrawCards();
        }
        
        
        private float ScreenHeight => Camera.main.orthographicSize * 2;

        private float ScreenWidth => ScreenHeight * (Screen.width / (float)Screen.height);

        public List<AbstractCard> HandCards => handCards;

        public GameObject DecisionCanvas
        {
            set => _decisionCanvas = value;
            get => _decisionCanvas;
        }

        public TeamCardManager TeamCardManager
        {
            get => _teamCardManager;
            set => _teamCardManager = value;
        }
    }
}