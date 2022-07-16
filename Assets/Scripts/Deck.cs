using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deck : MonoBehaviour
{
    // actual cards in the deck 0 ... first card to draw. 
    [SerializeField] private List<AbstractCard> deck = new();

    [SerializeField] private int maxDisplayedCardsInDeck = 4;
    [SerializeField] private float cardOffset = -0.07f;
    [SerializeField] private CardPlaceholder cardPlaceholderPrefab;

    [SerializeField] private CardBank cardBank;
    [SerializeField] private UpDownBumper drawIndicator;

    private static readonly System.Random _rng = new System.Random();

    private readonly List<CardPlaceholder> _cardPlaceholders = new();
    private int CurrentlyDisplayedCards => Math.Min(deck.Count, maxDisplayedCardsInDeck);
    private TeamCardManager _teamCardManager;
    
    public UpDownBumper DrawIndicator
    {
        get => drawIndicator;
    }
    
    public CardBank CardBank
    {
        get => cardBank;
        set => cardBank = value;
    }

    public TeamCardManager TeamCardManager
    {
        get => _teamCardManager;
        set => _teamCardManager = value;
    }

    private bool CanDrawCards => TeamCardManager.IsCardDrawEnabled;

    private void Start()
    {
        _initializeCardBacks();
    }

    private void OnMouseUp()
    {
        if (CanDrawCards)
            Draw();
    }

    public void Shuffle()
    {
        deck = deck.OrderBy(a => _rng.Next()).ToList();
    }

    public void AddCard(AbstractCard card)
    {
        deck.Add(card);
        RedrawCardBacks();
    }

    private bool IsCursorOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    /// <summary>
    ///     Draws the topmost card and returns it. Throws an NoMoreCardsException if there are no cards left.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NoMoreCardsException"></exception>
    public void Draw()
    {
        if (deck.Count == 0)
            throw new NoMoreCardsException();

        var drawnCard = deck[0];

        deck.RemoveAt(0);
        RedrawCardBacks();

        cardBank.AddCard(drawnCard);
        TeamCardManager.ReportDrawnCard(drawnCard);
    }

    private void RedrawCardBacks()
    {
        foreach (var cardPlaceholder in _cardPlaceholders)
            cardPlaceholder.gameObject.SetActive(false);

        for (var i = 0; i < Math.Min(CurrentlyDisplayedCards, _cardPlaceholders.Count); i++)
            _cardPlaceholders[i].gameObject.SetActive(true);
    }

    private void _initializeCardBacks()
    {
        for (var i = 0; i < maxDisplayedCardsInDeck; i++)
        {
            var placeholder = Instantiate(cardPlaceholderPrefab, transform);

            var posNew = transform.position;
            posNew.x += cardOffset * i;

            placeholder.transform.position = posNew;
            _cardPlaceholders.Add(placeholder);
        }

        RedrawCardBacks();
    }
}