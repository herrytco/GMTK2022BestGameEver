using System;
using Interfaces;
using UnityEngine;

namespace TileEffects
{
    public class PoisonTileEffect : ITileEffect
    {
        public override int Priority => 3;
 
        public override TileEffectResult OnOccupied(ITile tile, ICharacter visitor, Action onDone)
        {
            return new(false, true);
        }
    }
}