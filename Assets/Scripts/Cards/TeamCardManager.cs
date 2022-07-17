using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    /// <summary>
    /// Primary interface to the GameManager
    /// </summary>
    public class TeamCardManager : MonoBehaviour
    {
        [SerializeField] private List<AbstractCard> defaultCards;

        [SerializeField] private GameObject deckPrefab;
        [SerializeField] private Vector3 deckPosition;

        [SerializeField] private GameObject cardBankPrefab;

        [SerializeField] private int initialCardsOnHand = 6;

        private static readonly System.Random Rng = new();

        private Deck _deck;
        private CardBank _bank;
        private bool _isCardDrawEnabled = true;
        private bool _isCardPlayable = false;
        private Team _team;

        private readonly List<Action<AbstractCard>> _cardDrawCallbacks = new();

        private GameManager _gameManager;
        private GameObject _drawIndicator;
        private GameObject _decisionCanvas;

        public bool IsCardPlayable
        {
            get => _isCardPlayable;
            set
            {
                _isCardPlayable = value;
                foreach (var handCard in _bank.HandCards)
                {
                    handCard.Usable = value;
                }
            }
        }

        public bool IsCardDrawEnabled
        {
            get => _isCardDrawEnabled;
            set
            {
                _isCardDrawEnabled = value;
                _drawIndicator.SetActive(value);

                if (value)
                {
                    StartCoroutine("PrintDrawMessage");
                }
            }
        }

        private void Awake()
        {
            _deck = Instantiate(deckPrefab, transform).GetComponentInChildren<Deck>();
            _bank = Instantiate(cardBankPrefab, transform).GetComponentInChildren<CardBank>();
            _bank.TeamCardManager = this;

            _deck.transform.position = deckPosition;
            _deck.TeamCardManager = this;
            _deck.CardBank = _bank;

            _drawIndicator = _deck.DrawIndicator.gameObject;

            foreach (var card in defaultCards)
            {
                _deck.AddCard(card);
            }

            for (int i = 0; i < initialCardsOnHand; i++)
            {
                try
                {
                    _deck.Draw();
                }
                catch (NoMoreCardsException)
                {
                    Debug.LogError("More Initial Cards than there are default cards!");
                    break;
                }

                IsCardPlayable = false;
            }

            _deck.Shuffle();
        }

        private IEnumerator PrintDrawMessage()
        {
            yield return new WaitForSeconds(1f);

            _gameManager.TextManager.SetMessage("Draw a Card!");
        }

        public void AddDrawCallBack(Action<AbstractCard> callback)
        {
            _cardDrawCallbacks.Add(callback);
        }

        public void ReportDrawnCard(AbstractCard card)
        {
            IsCardDrawEnabled = false;
            IsCardPlayable = true;

            foreach (var cardDrawCallback in _cardDrawCallbacks)
            {
                cardDrawCallback(card);
            }
        }

        public void ReportSkillUse(AbstractCard card)
        {
            card.ExecuteEffect(_gameManager);
            Destroy(card.gameObject);
            _gameManager.PlayerUsedSkills(card);
        }

        public void ReportDiceRoll(AbstractCard card)
        {
            Destroy(card.gameObject);

            string diceString = card.GetCardData().DiceText;
            string[] parts = diceString.Split("d");

            int nRolls = int.Parse(parts[0]);
            int nSides = int.Parse(parts[1]);

            int result = 0;
            for (int i = 0; i < nRolls; i++)
            {
                result += Rng.Next(nSides) + 1;
            }

            _gameManager.PlayerUsedDice(result);
        }

        public GameObject DecisionCanvas
        {
            set
            {
                _decisionCanvas = value;
                _bank.DecisionCanvas = value;
            }
        }

        public GameManager GameManager
        {
            set => _gameManager = value;
        }

        public Team Team
        {
            get => _team;
            set => _team = value;
        }
    }
}