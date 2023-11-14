using System.Collections;
using Cards;
using TMPro;
using UI.Elements.ProgressBar;
using UnityEngine;

namespace UI.GUI.BattleGUI
{
    public class BattleGUI: MonoBehaviour
    {
        public bool qteIsOver;
        public Transform placeholder;

        [SerializeField] private TextMeshPro logText;
        private ProgressBar _progressBar;

        private void Awake()
        {
            _progressBar = GetComponentInChildren<ProgressBar>();
        }

        public void StartQte(float maxSeconds, float? delay = null)
        {
            StartCoroutine(StartQteAsync(maxSeconds, delay));
        }

        public void DisableQteProgressBar()
        {
            StopAllCoroutines();
            _progressBar.gameObject.SetActive(false);
        }
        
        public IEnumerator StartQteAsync(float maxSeconds, float? delay = null)
        {
            float progressValue = 0;
            yield return new WaitForSeconds(delay ?? 0);
            _progressBar.gameObject.SetActive(true);
            while (progressValue < maxSeconds)
            {
                progressValue += Time.deltaTime;
                var progressFactor = _progressBar.PositionManagement(progressValue, maxSeconds, true);
                _progressBar.ColorManagement(progressFactor);
                yield return null;
            }
            qteIsOver = true;
        }

        public void WriteLog(int round, ICard.TypeEnum monsterAttackType, int monsterForce)
        {
            logText.text = $"round : {round},\n attack : {monsterAttackType},\n force : {monsterForce}";
        }
    }
}