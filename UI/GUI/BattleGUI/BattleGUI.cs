using System;
using UI.Elements.ProgressBar;
using UnityEngine;

namespace UI.GUI.BattleGUI
{
    public class BattleGUI: MonoBehaviour
    {
        public float timer;
        public Transform placeholder;
        
        private ProgressBar _progressBar;

        private void Awake()
        {
            _progressBar = GetComponentInChildren<ProgressBar>();
        }

        public void StartQte(float maxSeconds)
        {
            StartCoroutine(_progressBar.StartQteProgress(maxSeconds));
        }
    }
}