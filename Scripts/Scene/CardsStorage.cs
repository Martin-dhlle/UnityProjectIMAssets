using System;
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
        
        private Dictionary<GameObject, Card> _cards = new();
        
        private void Awake()
        {
            InstantiateAllCards();
        }

        /// <summary>
        /// Instantiate all cards by type
        /// </summary>
        private void InstantiateAllCards()
        {
            cardPrefab.SetActive(false);
            for (var i = 0; i < ((ICard.TypeEnum[])Enum.GetValues(typeof(ICard.TypeEnum))).Length; i++)
            {
                var cardInstance = Instantiate(cardPrefab);
                var cardData = cardInstance.GetComponent<Card>();
                (cardData.Type, cardData.Force) = ((ICard.TypeEnum)i, forceInit[i]);
                _cards.Add(cardInstance, cardData);
            }
        }

        private void SetCard(GameObject key, int forceToAdd)
        {
            _cards[key].Force += forceToAdd;
        }

        private GameObject[] GetAllCards()
        {
            return _cards.Keys.ToArray();
        }
    }
}