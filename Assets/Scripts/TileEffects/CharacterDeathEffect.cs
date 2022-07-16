using System;
using Interfaces;
using UnityEngine;

namespace TileEffects
{
    public class CharacterDeathEffect : ITileEffect
    {
        public override int Priority => int.MaxValue;

        public override TileEffectResult OnOccupied(ITile tile, ICharacter visitor, Action onDone)
        {
            // TODO implement dramatic character death
            Debug.Log($"Character {tile.Characters[0].Name} got crushed by {visitor.Name}");
            return new(false, true);
        }
    }
}