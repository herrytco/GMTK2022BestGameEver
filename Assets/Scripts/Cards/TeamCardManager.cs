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

        private Deck _deck;
        private CardBank _bank;
        private bool _isCardDrawEnabled = true;
        private Team _team;

        private readonly List<Action<AbstractCard>> _cardDrawCallbacks = new();

        private GameManager _gameManager;

        private GameObject _drawIndicator;

        public GameManager GameManager
        {
            set => _gameManager = value;
        }

        public Team Team
        {
            get => _team;
            set => _team = value;
        }

        public bool IsCardDrawEnabled
        {
            get => _isCardDrawEnabled;
            set
            {
                _isCardDrawEnabled = value; 
                _drawIndicator.SetActive(value);
            }
        }

        private void Awake()
        {
            _deck = Instantiate(deckPrefab, transform).GetComponentInChildren<Deck>();
            _bank = Instantiate(cardBankPrefab, transform).GetComponentInChildren<CardBank>();

            _deck.transform.position = deckPosition;
            _deck.TeamCardManager = this;
            _deck.CardBank = _bank;

            _drawIndicator = _deck.DrawIndicator.gameObject;
            Debug.Log(_drawIndicator.name);

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
            }
            
            _deck.Shuffle();
        }

        private void Start()
        {
            
        }

        public void AddDrawCallBack(Action<AbstractCard> callback)
        {
            _cardDrawCallbacks.Add(callback);
        }
        
        public void ReportDrawnCard(AbstractCard card)
        {
            IsCardDrawEnabled = false;
            
            foreach (var cardDrawCallback in _cardDrawCallbacks)
            {
                cardDrawCallback(card);
            }
        }
    }
}