using System.Collections;
using UnityEngine;

namespace UI.Elements.TextBoxes
{
    public class Fade : MonoBehaviour
    {
        public float delayInSecondBeforeFade;
        public float delayInSecondBetweenFadeFrame;
        private Renderer[] _renderers;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();
        }

        private void Start()
        {
            StartCoroutine(FadeRenderers());
        }

        private IEnumerator FadeRenderers()
        {
            // animation
            yield return new WaitForSeconds(delayInSecondBeforeFade);
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
