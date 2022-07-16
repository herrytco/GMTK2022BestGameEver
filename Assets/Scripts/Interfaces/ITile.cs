using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TileEffects;
using UnityEngine;

namespace Interfaces
{
    public abstract class ITile : MonoBehaviour
    {
        protected List<ITileEffect> _activeEffects;
        public List<ITile> NextTiles { get; protected set; } = new();
        public List<ITile> PrevTiles { get; protected set; } = new();

        public List<ICharacter> Characters { get; protected set; } = new();

        public List<ITileEffect> ActiveEffects => _activeEffects;

        public void AddEffect(ITileEffect effect)
        {
            _activeEffects.Add(effect);
            // TODO check if this sorts the right way around
            _activeEffects.Sort(Comparer<ITileEffect>.Create((a, b) =>
                -a.Priority.CompareTo(b.Priority)));
        }

        public void RemoveEffectsWhere(Predicate<ITileEffect> match)
        {
            _activeEffects.RemoveAll(match);
        }

        /// <summary>
        ///     A character passes through this tile (does not stay/try to occupy)
        /// </summary>
        public void Visit(ICharacter visitor, [NotNull] Action<ITile, ICharacter> onDone)
        {
            HandleEffectAction = effect =>
            {
                return effect.OnVisit(this, visitor, () => StartCoroutine(HandleEffects()));
            };

            OnEffectsHandled = () => onDone(this, visitor);

            StopCoroutine(HandleEffects());
            StartCoroutine(HandleEffects());
        }

        /// <summary>
        ///     Called when a character tries to occupy this tile, but it already is occupied.
        /// </summary>
        /// <param name="defenders">
        ///     The characters already present on this tile.
        ///     Assumed to be non-empty.
        /// </param>
        /// <param name="attacker">The visiting character.</param>
        /// <returns>True if the attacker wins and occupies the tile.</returns>
        public bool Fight(List<ICharacter> defenders, ICharacter attacker)
        {
            ITileEffect deathEffect = defenders[0].gameObject.AddComponent<CharacterDeathEffect>();
            AddEffect(deathEffect);

            Characters = new List<ICharacter> { attacker };

            return true;
        }

        /// <summary>
        ///     A character tries to occupy this tile.
        /// </summary>
        public void Occupy(ICharacter attacker, [NotNull] Action<ITile, ICharacter> onDone)
        {
            // if the tile is occupied and the visitor loses the fight, do not process effects
            if (Characters.Count != 0 && !Fight(Characters, attacker))
            {
                onDone(this, attacker);
                return;
            }

            HandleEffectAction = effect =>
            {
                return effect.OnOccupied(this, attacker, () => StartCoroutine(HandleEffects()));
            };

            OnEffectsHandled = () => onDone(this, attacker);

            StopCoroutine(HandleEffects());
            StartCoroutine(HandleEffects());
        }

        /// <summary>
        ///     A character (that has occupied this tile previously) leaves this tile.
        /// </summary>
        public void Leave(ICharacter character, ITile destination, [NotNull] Action<ITile, ICharacter> onDone)
        {
            HandleEffectAction = effect =>
            {
                return effect.OnLeave(this, character, destination, () => StartCoroutine(HandleEffects()));
            };

            OnEffectsHandled = () => onDone(this, character);

            StopCoroutine(HandleEffects());
            StartCoroutine(HandleEffects());

            Characters.RemoveAll(c => c == character);
        }

        public Action OnEffectsHandled
        {
            get => _onEffectsHandled;
            protected set
            {
                if (value != null && _onEffectsHandled != null)
                    Debug.LogWarning(
                        "replacing existing OnEffectsHandled; coroutine may not have terminated correctly");
                if (value == null)
                    StopCoroutine(HandleEffects());
                _onEffectsHandled = value;
            }
        }

        private Action _onEffectsHandled;

        protected Func<ITileEffect, TileEffectResult> HandleEffectAction;

        protected IEnumerator HandleEffects()
        {
            foreach (var effect in _activeEffects.ToList())
            {
                TileEffectResult result = HandleEffectAction(effect);
                if (!result.Done) yield return null; // pause execution until next call
                
                Destroy(effect.gameObject);
                
                if (result.CharacterRemoved) break;
            }

            var onEffectsHandled = _onEffectsHandled;
            _onEffectsHandled = null;
            onEffectsHandled();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}