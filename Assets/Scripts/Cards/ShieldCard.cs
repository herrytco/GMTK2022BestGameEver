using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class ShieldCard : AbstractCard
    {
        [SerializeField] private ShieldCardData data;
        
        public override CardData GetCardData()
        {
            return data;
        }

        public override void ExecuteEffect()
        {
        }
    }
}
