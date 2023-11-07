using System;
using UnityEngine;

namespace UI.Elements.ProgressBar
{
    public class ProgressBar : MonoBehaviour
    {
        public float progressValue, maxProgressValue, minYPos, maxYPos;
        public Color startColor, endColor;
        public Transform progressBarDeformer;
        public GameObject progressBar;

        private Renderer _rend;
        private Color _currentColor;

        private void Start()
        {
            _rend = progressBar.GetComponent<Renderer>();
            _currentColor = _rend.material.color;
        }

        private void Update()
        {
            if (!(progressValue > 0)) return;
            PositionManagement();
            ColorManagement();
        }

        private void PositionManagement()
        {
            var progressFactor = Mathf.InverseLerp(0, maxProgressValue, progressValue);
            var pos = progressBarDeformer.position;
            pos.y = Mathf.Lerp(minYPos, minYPos, progressFactor);
            progressBarDeformer.position = pos;
        }

        private void ColorManagement()
        {
            _currentColor = Color.Lerp(startColor, endColor, 1);
            _rend.material.color = _currentColor;
        }
    }
}
