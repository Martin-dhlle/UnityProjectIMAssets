using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tests
{
    public class TestFameValue
    {
        
        private static int[] _fameValues = { 50, 20, 80, 0, 150 };

        private static List<int> GenerateValues()
        {
            var fameValues = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                fameValues.Add(Random.Range(0, 150));
            }

            return fameValues;
        }
        
        private static int AddPlayerFame(int fameToAdd, int playerFame)
        {
            var result = playerFame + fameToAdd;
            if (result is > 150 or < 0) return playerFame;
            playerFame = result;
            return playerFame;
        }
        
        private static int AddForceToSingleCard(int cardForce, int forceToAdd)
        {
            if (cardForce + forceToAdd is > 150 or < 0) return cardForce;
            return cardForce + forceToAdd;
        }
        
        // A unit test concerning the increment and decrement part of the fame value
        [Test]
        public void TestFameValueIncrement([ValueSource(nameof(GenerateValues))] int fameValue,
            [ValueSource(nameof(GenerateValues))] int cardForceValue)
        {
            var dynamicFame = fameValue;
            var dynamicCardForce = cardForceValue;
            
            switch(Mathf.Sign(Random.Range(float.NegativeInfinity, float.PositiveInfinity)) > 0 ? float.PositiveInfinity : float.NegativeInfinity)
            {
                case float.PositiveInfinity:
                    if ((dynamicFame = AddPlayerFame(-10, dynamicFame)) != fameValue)
                    {
                        if ((dynamicCardForce = AddForceToSingleCard(dynamicCardForce, 10)) == cardForceValue)
                        {
                            dynamicFame = AddPlayerFame(10, dynamicFame);
                            return;
                        }
                    }
                    break;
                case float.NegativeInfinity:
                    if ((dynamicFame = AddPlayerFame(10, dynamicFame)) != fameValue)
                    {
                        if ((dynamicCardForce = AddForceToSingleCard(dynamicCardForce, -10)) == cardForceValue)
                        {
                            dynamicFame = AddPlayerFame(-10, dynamicFame);
                            return;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Assert.LessOrEqual(dynamicFame, 150);
            Assert.LessOrEqual(cardForceValue, 150);
            Assert.GreaterOrEqual(dynamicFame, 0);
            Assert.GreaterOrEqual(cardForceValue, 0);
        }
    }
}
