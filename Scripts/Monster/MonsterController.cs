using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Monster.Json;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monster
{
    public class MonsterController: MonoBehaviour
    {
        [SerializeField] private List<TextAsset> patternList;
        [SerializeField] private AudioClip meowLose, meowWin;
        private AudioSource _audioSource;
        
        private Animator _animator;
        private Dictionary<string, JsonMonsterData> _jsonMonsterData;
        private static readonly int State = Animator.StringToHash("animationState");

        public enum MonsterAnimationEnum
        {
            Default, Meow, Move, Slash, Thrust, Bash
        }
        
        public enum AudioEnum
        {
            Lose, Win
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
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
            return _jsonMonsterData[currentRound.ToString()].Force;
        }

        public int GetFame(int currentRound)
        {
            return _jsonMonsterData[currentRound.ToString()].Fame;
        }

        public ICard.TypeEnum GetAttackType(int currentRound)
        {
            return Enum.TryParse<ICard.TypeEnum>(_jsonMonsterData[currentRound.ToString()].Type, out var result) 
                ? result
                : ICard.TypeEnum.Bash;
        }

        /// <summary>
        /// Get the entire pattern dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, JsonMonsterData> GetPatternInfos()
        {
            return _jsonMonsterData;
        }

        /// <summary>
        /// Random generation of a pattern, called every stages except the first one
        /// </summary>
        public void GenerateRandomPattern()
        {
            _jsonMonsterData = JsonConvert.DeserializeObject<Dictionary<string, 
                JsonMonsterData>>(patternList[Random.Range(1, patternList.Count - 1)].text);
        }

        public void Animate(MonsterAnimationEnum monsterAnimation)
        {
            _animator.speed = 1;
            _animator.SetInteger(State, (int)monsterAnimation);
        }
        
        public void AnimateAttack(ICard.TypeEnum monsterAttackType, float duration = 1)
        {
            var monsterAnimation = monsterAttackType switch
            {
                ICard.TypeEnum.Slash => MonsterAnimationEnum.Slash,
                ICard.TypeEnum.Bash => MonsterAnimationEnum.Bash,
                ICard.TypeEnum.Thrust => MonsterAnimationEnum.Thrust,
                _ => throw new Exception()
            };
            _animator.speed /= duration / 2;
            _animator.SetInteger(State, (int)monsterAnimation);
        }

        public IEnumerator PlayAudio(AudioEnum audioType, float? delay = null)
        {
            yield return new WaitForSeconds(delay ?? 0);
            switch (audioType)
            {
                case AudioEnum.Win:
                    _audioSource.PlayOneShot(meowWin);
                    break;
                case AudioEnum.Lose:
                    _audioSource.PlayOneShot(meowLose);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null);
            }

            yield return null;
        }
    }
}