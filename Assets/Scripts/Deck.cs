using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEditor;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // actual cards in the deck 0 ... first card to draw. 
    [SerializeField] private List<AbstractCard> deck = new List<AbstractCard>();
    
    [SerializeField] private int maxDisplayedCardsInDeck = 4;
    [SerializeField] private float cardOffset = -0.07f;
    [SerializeField] private CardPlaceholder cardPlaceholderPrefab;
    [SerializeField] private CardBank cardBank;

    private int CurrentlyDisplayedCards => Math.Min(deck.Count, maxDisplayedCardsInDeck);
    private readonly List<CardPlaceholder> _cardPlaceholders = new List<CardPlaceholder>();

    private bool _canDrawCards = true;

    private void Start()
    {
        _initializeCardBacks();
    }
    
    void Update() {  
        if (_canDrawCards && Input.GetMouseButtonDown(0))
        {
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  
            RaycastHit2D hit = Physics2D.Raycast(cursorPosition, Vector3.back);  
            
            if (hit.collider != null)
            {
                Draw();
            }  
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
    /// Draws the topmost card and returns it. Throws an NoMoreCardsException if there are no cards left. 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NoMoreCardsException"></exception>
    public void Draw()
    {
        if(deck.Count == 0)
            throw new NoMoreCardsException();
        
        AbstractCard drawnCard = deck[0];

        deck.RemoveAt(0);
        RedrawCardBacks();

        cardBank.AddCard(drawnCard);
    }
    
    public void RedrawCardBacks()
    {
        foreach (var cardPlaceholder in _cardPlaceholders)
            cardPlaceholder.gameObject.SetActive(false);
        
        for (int i = 0; i < CurrentlyDisplayedCards; i++)
            _cardPlaceholders[i].gameObject.SetActive(true);
    }

    private void _initializeCardBacks()
    {
        for (int i = 0; i < maxDisplayedCardsInDeck; i++)
        {
            CardPlaceholder placeholder = Instantiate(cardPlaceholderPrefab, transform);

            Vector3 posNew = transform.position;
            posNew.x += cardOffset * i;

            placeholder.transform.position = posNew;
            _cardPlaceholders.Add(placeholder);
        }

        RedrawCardBacks();
    }
    
}