using System.Collections;
using UnityEngine;

namespace UI.Elements.TextBoxes
{
    public class BackgroundTransition : MonoBehaviour
    {
        public float delayInSecondBetweenFadeFrame;
        private Renderer[] _renderers;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();
        }

         public void ResetRenderersAlpha()
        {
            foreach (var rend in _renderers)
            {
                var material = rend.material;
                var materialColor = material.color;
                materialColor.a = 1;
                material.color = materialColor;
            }
        }

        public IEnumerator FadeRenderers()
        {
            yield return new WaitForSeconds(0.5f);
            for (var time = 1; time < 10; time++)
            {
                foreach (var rend in _renderers)
                {
                    var materialColor = rend.material.color;
                    materialColor.a = Mathf.Clamp(1 - Mathf.InverseLerp(0, 9, time), 0, 1);
                    rend.material.color = materialColor;
                }
                yield return new WaitForSeconds(delayInSecondBetweenFadeFrame);
            }
        }
    }
}
