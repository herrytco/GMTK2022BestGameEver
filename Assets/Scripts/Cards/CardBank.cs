using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardBank : MonoBehaviour
    {
        [SerializeField] private List<AbstractCard> handCards = new();

        public void AddCard(AbstractCard card)
        {
            handCards.Add(Instantiate(card));
            RedrawCards();
        }

        public void RedrawCards()
        {
            for (var i = 0; i < handCards.Count; i++)
            {
                var handCard = handCards[i];

                var posNew = Camera.main.ScreenToWorldPoint(
                    new Vector2(0, 0)
                );

                var cardSize = handCard.GetComponentInChildren<Renderer>().bounds.size;

                posNew.z = 0;
                posNew.x += cardSize.x / 2 + i * cardSize.x;
                posNew.y += cardSize.y / 2;

                handCard.transform.position = posNew;
            }
        }
    }
}