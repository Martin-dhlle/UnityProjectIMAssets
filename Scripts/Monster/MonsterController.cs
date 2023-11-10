using System;
using System.Collections.Generic;
using Cards;
using Monster.Json;
using Newtonsoft.Json;
using UnityEngine;

namespace Monster
{
    public class MonsterController: MonoBehaviour
    {
        [SerializeField] private List<TextAsset> patternList; // each patterns = stage
        
        private Animator _animator;
        private Dictionary<string, JsonMonsterData> _jsonMonsterData;
        
        private void Awake()
        {
            _jsonMonsterData = JsonConvert.DeserializeObject<Dictionary<string, JsonMonsterData>>(patternList[0].text);
        }

        /// <summary>
        /// Get the QTE time value along with the current round.
        /// Set the animator animation speed value to slow down
        /// or speed up the monster attack speed.
        /// </summary>
        /// <param name="currentRound"></param>
        /// <returns>Qte count from pattern</returns>
        public int GetQte(int currentRound)
        {
            var qte = _jsonMonsterData[currentRound.ToString()].Qte;
            // set animator speed value from qte
            return qte;
        }
        
        public int GetForce(int currentRound)
        {
            return _jsonMonsterData[currentRound.ToString()].Qte;
        }

        public ICard.TypeEnum GetAttackType(int currentRound)
        {
            return Enum.TryParse<ICard.TypeEnum>(_jsonMonsterData[currentRound.ToString()].Type, out var result) 
                ? result
                : ICard.TypeEnum.Bash;
        }

        /// <summary>
        /// Random generation of a pattern, called every stages except the first one
        /// </summary>
        public void GeneratePattern()
        {
            
        }
    }
}