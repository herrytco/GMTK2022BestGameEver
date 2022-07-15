using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

namespace Cards
{
    public class CardBank : MonoBehaviour
    {
        [SerializeField] private List<AbstractCard> handCards = new List<AbstractCard>();

        public void AddCard(AbstractCard card)
        {
            handCards.Add(Instantiate(card));
            RedrawCards();
        }

        public void RedrawCards()
        {

            for (int i = 0; i < handCards.Count; i++)
            {
                AbstractCard handCard = handCards[i];
                
                Vector3 posNew = Camera.main.ScreenToWorldPoint(
                    new Vector2(0, 0)
                );

                Vector3 cardSize = handCard.GetComponentInChildren<Renderer>().bounds.size;

                posNew.z = 0;
                posNew.x += cardSize.x / 2 + i * cardSize.x;
                posNew.y += cardSize.y / 2;

                handCard.transform.position = posNew;
            }
        }
    }
}
