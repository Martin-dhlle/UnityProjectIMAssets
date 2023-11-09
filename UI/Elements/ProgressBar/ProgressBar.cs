using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Elements.ProgressBar
{
    public class ProgressBar : MonoBehaviour
    {
        public float minYPos, maxYPos;
        public Color startColor, endColor;
        [SerializeField] private  Transform progressBarDeform;
        [SerializeField] private GameObject progressBar;

        private Renderer _rend;
        private Color _currentColor;

        private void Start()
        {
            _rend = progressBar.GetComponent<Renderer>();
            _currentColor = _rend.material.color;
        }

        private float PositionManagement(float currentProgressValue,float maxProgressValue)
        {
            var progressInterpolation = Mathf.InverseLerp(0, maxProgressValue, currentProgressValue);
            var pos = progressBarDeform.position;
            pos.y = Mathf.Lerp(minYPos, maxYPos, progressInterpolation);
            progressBarDeform.position = pos;
            return progressInterpolation;
        }

        private void ColorManagement(float currentLinearInterpolation)
        {
            _currentColor = Color.Lerp(startColor, endColor, currentLinearInterpolation);
            _rend.material.color = _currentColor;
        }
        
        public IEnumerator StartQteProgress(float maxSeconds)
        {
            float progressValue = 0;
            for (var i = 0; i < maxSeconds; i++)
            {
                progressValue += Time.deltaTime;
                var progressFactor = PositionManagement(progressValue, maxSeconds);
                ColorManagement(progressFactor);
                yield return null;
            }
            // declare defeat of the round
        }
    }
}
