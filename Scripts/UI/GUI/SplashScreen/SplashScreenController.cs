using System;
using System.Collections;
using UnityEngine;

namespace UI.start_splashscreen
{
    public class SplashScreenController : MonoBehaviour
    {
        public GameObject  firstSplashScreen, secondSplashScreen, introductionCamera;
        public AudioClip step1, step2;

        private Animator _cameraAnimator;
        private AudioSource _audioSource;
        private Renderer _rend;
        private static readonly int CanMove = Animator.StringToHash("canMove");

        // Start is called before the first frame update

        private void Awake()
        {
            _cameraAnimator = introductionCamera.GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _rend = GetComponent<Renderer>();
            firstSplashScreen.SetActive((false));
            secondSplashScreen.SetActive((false));
        }

        private void Start()
        {
            StartCoroutine(StartIntroduction());
        }

        private IEnumerator StartIntroduction()
        {
            yield return new WaitForSeconds(2);
            firstSplashScreen.SetActive((true));
            _audioSource.PlayOneShot(step1); // play monster footstep sound
            yield return new WaitForSeconds(4);
            firstSplashScreen.SetActive((false));
            secondSplashScreen.SetActive((true));
            _audioSource.PlayOneShot(step2); // play another monster footstep sound
            yield return new WaitForSeconds(4);
            var matColor = _rend.material.color;
            // transition fading splashscreen background
            _cameraAnimator.SetBool(CanMove, true);
            yield return new WaitForSeconds(1);
            secondSplashScreen.SetActive((false));
            for (float time = 1; time < 10; time ++)
            {
                matColor.a =  Mathf.Clamp(1 - Mathf.InverseLerp(0, 10, time), 0, 1);
                _rend.material.color = matColor;
                yield return new WaitForSeconds(0.1f);
            }
            yield break;
        }
    }
}
