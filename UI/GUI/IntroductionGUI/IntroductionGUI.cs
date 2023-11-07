using System;
using System.Collections;
using UnityEngine;

namespace UI.GUI.IntroductionGUI
{
    public class IntroductionGUI : MonoBehaviour
    {
        [NonSerialized] public Transform CoordinatesToEndIntro;
        public GameObject  firstSplashScreen, secondSplashScreen, background;
        public AudioClip step1, step2;
        public bool isIntroOver;

        private Camera _camera;
        private Animator _cameraAnimator;
        private AudioSource _audioSource;
        private Renderer _rend;
        private static readonly int CanMove = Animator.StringToHash("canMove");

        private void Awake()
        {
            _camera = Camera.main;
            _cameraAnimator = _camera!.GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _rend = background.GetComponent<Renderer>();
            
            firstSplashScreen.SetActive((false));
            secondSplashScreen.SetActive((false));
        }

        private void Start()
        {
            StartCoroutine(StartIntroduction());
        }

        private void Update()
        {
            if (!firstSplashScreen.activeSelf) return;
            MoveText();
            CheckIntroState();
        }

        private IEnumerator StartIntroduction()
        {
            yield return new WaitForSeconds(2);
            firstSplashScreen.SetActive((true));
            _audioSource.PlayOneShot(step1); // play monster footstep sound
            yield return new WaitForSeconds(3);
            firstSplashScreen.SetActive((false));
            secondSplashScreen.SetActive((true));
            _audioSource.PlayOneShot(step2); // play another monster footstep sound
            yield return new WaitForSeconds(2);
            var matColor = _rend.material.color;
            // transition fading splashscreen background
            _cameraAnimator.SetBool(CanMove, true);
            yield return new WaitForSeconds(1);
            secondSplashScreen.SetActive((false));
            for (float time = 1; time < 10; time ++)
            {
                matColor.a = Mathf.Clamp(1 - Mathf.InverseLerp(0, 10, time), 0, 1);
                _rend.material.color = matColor;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void MoveText()
        {
            var textTransformPosition = firstSplashScreen.transform.localPosition;
            
            textTransformPosition.y = Mathf.Clamp(
                textTransformPosition.y - 1, 
                background.transform.localPosition.y + 1,
                float.PositiveInfinity);
            
            firstSplashScreen.transform.localPosition = textTransformPosition;
        }

        private void CheckIntroState()
        {
            if(!(Mathf.Round(_camera.transform.position.z) < Mathf.Floor(CoordinatesToEndIntro.position.z))) isIntroOver = true;
        }
    }
}
