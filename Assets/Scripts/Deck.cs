using System;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // actual cards in the deck 0 ... first card to draw. 
    [SerializeField] private List<AbstractCard> deck = new();

    [SerializeField] private int maxDisplayedCardsInDeck = 4;
    [SerializeField] private float cardOffset = -0.07f;
    [SerializeField] private CardPlaceholder cardPlaceholderPrefab;
    [SerializeField] private CardBank cardBank;
    private readonly List<CardPlaceholder> _cardPlaceholders = new();

    private bool _canDrawCards = true;

    private int CurrentlyDisplayedCards => Math.Min(deck.Count, maxDisplayedCardsInDeck);

    private void Start()
    {
        _initializeCardBacks();
    }

    private void Update()
    {
        if (_canDrawCards && Input.GetMouseButtonDown(0))
        {
            var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(cursorPosition, Vector3.back);

            if (hit.collider != null) Draw();
        }
    }

    public void DisableCardDraw()
    {
        _canDrawCards = false;
    }

    public void EnableCardDraw()
    {
        _canDrawCards = true;
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
    }

    public void RedrawCardBacks()
    {
        foreach (var cardPlaceholder in _cardPlaceholders)
            cardPlaceholder.gameObject.SetActive(false);

        for (var i = 0; i < CurrentlyDisplayedCards; i++)
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