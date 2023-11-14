using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;

namespace Scene
{
    public class CardsStorage: MonoBehaviour
    {
        public GameObject cardPrefab;
        public int[] forceInit;
        public int playerFame;
        
        private readonly Dictionary<GameObject, Card> _cards = new();
        private readonly Dictionary<GameObject, Animator> _cardsAnimator = new();
        private static readonly int State = Animator.StringToHash("animationState");

        // Animation types for cards
        public enum AnimationTypeEnum {Disappear, Spawn, Focus}
        
        private void Awake()
        {
            InstantiateAllCards();
        }

        /// <summary>
        /// Get all cards classes running
        /// </summary>
        /// <returns>A list of cards</returns>
        public List<Card> GetAllCards()
        {
            return _cards.Values.ToList();
        }

        /// <summary>
        /// Instantiate all cards by type.
        /// Cards are not active and are instantiate first with the main camera as the transform parent.
        /// </summary>
        private void InstantiateAllCards()
        {
            cardPrefab.SetActive(false);
            for (var i = 0; i < ((ICard.TypeEnum[])Enum.GetValues(typeof(ICard.TypeEnum))).Length; i++)
            {
                var cardInstance = Instantiate(cardPrefab, Camera.main!.transform);
                var cardData = cardInstance.GetComponent<Card>();
                var cardAnimator = cardInstance.GetComponent<Animator>();
                
                cardData.ChangeType((ICard.TypeEnum)i);
                cardData.AddForce(forceInit[i]);
                
                _cards.Add(cardInstance, cardData);
                _cardsAnimator.Add(cardInstance, cardAnimator);
            }
        }

        /// <summary>
        /// Define the new Vector3 for cards and activate cards game objects.
        /// </summary>
        /// <param name="centerPointSpawn">The center where cards spawn</param>
        public void SpawnAllCardsInScene(Vector3 centerPointSpawn)
        {
            var incrementVector = -0.9f;
            foreach (var card in _cards)
            {
                card.Key.transform.position = centerPointSpawn;
                card.Key.transform.localPosition += Vector3.right * incrementVector;
                card.Key.transform.localRotation = Quaternion.identity * Quaternion.Euler(-90,0,-90);
                card.Key.SetActive(true);
                incrementVector += 0.9f;
            }
        }

        /// <summary>
        /// Disable all cards gameObject on the scene
        /// </summary>
        public void DespawnAllCards()
        {
            foreach (var card in _cards)
            {
                card.Key.SetActive(false);
            }
        }
        
        public void RespawnAllCards()
        {
            foreach (var card in _cards)
            {
                card.Value.UpdateText();
                card.Key.SetActive(true);
            }
        }

        public int GetCardForce(GameObject card)
        {
            return _cards[card].Force;
        }
        
        public void AddForceToSingleCard(GameObject key, int forceToAdd)
        {
            _cards[key].AddForce(forceToAdd);
        }

        /// <summary>
        /// Animate a card with chosen animation type by his gameObject key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="animationType"></param>
        public void AnimateSingle(GameObject key, AnimationTypeEnum animationType)
        {
            _cardsAnimator[key].SetInteger(State, (int)animationType);
        }

        public void AnimateMany(AnimationTypeEnum animationType)
        {
            foreach (var cAnimator in _cardsAnimator)
            {
                cAnimator.Value.SetInteger(State, (int)animationType);
            }
        }

        public IEnumerator AnimateManyAsync(AnimationTypeEnum animationType, float delayBetweenInSeconds, float? delayBeforeInSeconds = null)
        {
            yield return new WaitForSeconds(delayBeforeInSeconds ?? 0);
            foreach (var cAnimator in _cardsAnimator)
            {
                cAnimator.Value.SetInteger(State, (int)animationType);
                yield return new WaitForSeconds(delayBetweenInSeconds);
            }
        }

        public void AddPlayerFame(int fameFromPattern)
        {
            playerFame += fameFromPattern;
        }

        public ICard.TypeEnum GetCardType(GameObject card)
        {
            return _cards[card].Type;
        }
    }
}