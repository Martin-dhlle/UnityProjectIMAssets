using UnityEngine;

namespace UI.Elements.ProgressBar
{
    public class ProgressBar : MonoBehaviour
    {
        public float minYPos, maxYPos;
        [SerializeField] private Color startColor, endColor;
        [SerializeField] private  Transform progressBarDeform;
        [SerializeField] private GameObject progressBar;
        private Renderer _rend;

        private void Start()
        {
            _rend = progressBar.GetComponent<Renderer>();
            Debug.Log(_rend.material.color);
        }

        public float PositionManagement(float currentProgressValue,float maxProgressValue)
        {
            var progressInterpolation = 1 - Mathf.InverseLerp(0, maxProgressValue, currentProgressValue);
            var pos = progressBarDeform.localPosition;
            pos.y = Mathf.Lerp(minYPos, maxYPos, progressInterpolation);
            progressBarDeform.localPosition = pos;
            return progressInterpolation;
        }

        public void ColorManagement(float currentLinearInterpolation)
        {
            var currentColor = Color.Lerp(startColor, endColor, currentLinearInterpolation);
            
            /*_rend.material.color = currentColor;*/
        }
    }
}
