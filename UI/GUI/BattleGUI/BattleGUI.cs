using System.Collections;
using UI.Elements.ProgressBar;
using UnityEngine;

namespace UI.GUI.BattleGUI
{
    public class BattleGUI: MonoBehaviour
    {
        public bool qteIsOver;
        public Transform placeholder;
        
        private ProgressBar _progressBar;

        private void Awake()
        {
            _progressBar = GetComponentInChildren<ProgressBar>();
        }

        public void DisableQteProgressBar()
        {
            _progressBar.gameObject.SetActive(false);
        }
        
        public IEnumerator StartQte(float maxSeconds, float? delay = null)
        {
            float progressValue = 0;
            yield return new WaitForSeconds(delay ?? 0);
            _progressBar.gameObject.SetActive(true);
            while (progressValue < maxSeconds)
            {
                progressValue += Time.deltaTime;
                var progressFactor = _progressBar.PositionManagement(progressValue, maxSeconds);
                _progressBar.ColorManagement(progressFactor);
                yield return null;
            }
            qteIsOver = true;
        }
    }
}