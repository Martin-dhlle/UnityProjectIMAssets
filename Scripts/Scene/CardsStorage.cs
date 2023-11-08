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
        
        private readonly Dictionary<GameObject, Card> _cards = new();
        private readonly Dictionary<GameObject, Animator> _cardsAnimator = new();
        
        // Animation types for a single card
        public enum SingleAnimationTypeEnum {Focus, Disappear}
        
        // Animation types for many cards
        public enum AnimationTypeEnum {Spawn, Disappear}
        
        private void Awake()
        {
            InstantiateAllCards();
        }

        /// <summary>
        /// Instantiate all cards by type.
        /// Cards are not active and are instantiate with the main camera as the transform parent.
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
                card.Key.SetActive(true);
                incrementVector += 0.9f;
            }
        }

        public void AddForceToSingleCard(GameObject key, int forceToAdd)
        {
            _cards[key].AddForce(forceToAdd);
        }

        public GameObject[] GetAllCards()
        {
            return _cards.Keys.ToArray();
        }

        public void AnimateSingle(GameObject key, SingleAnimationTypeEnum animationType)
        {
            _cardsAnimator[key].SetBool($"can{animationType.ToString()}", true);
        }

        public void AnimateMany(AnimationTypeEnum animationType)
        {
            foreach (var cAnimator in _cardsAnimator)
            {
                cAnimator.Value.SetBool($"can{animationType.ToString()}", true);
            }
        }

        public IEnumerator AnimateManyAsync(AnimationTypeEnum animationType, int timeInSecond)
        {
            foreach (var cAnimator in _cardsAnimator)
            {
                cAnimator.Value.SetBool($"can{animationType.ToString()}", true);
                yield return new WaitForSeconds(timeInSecond);
            }
        }
    }
}