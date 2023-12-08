using System;
using TMPro;
using UI.Elements.ProgressBar;
using UnityEngine;

namespace UI.GUI.PreparationGUI
{
    public class PreparationGUI: MonoBehaviour
    {
        public Transform cardsPlaceholder, cardsInfoPlaceholder;
        
        [SerializeField] private GameObject progressBarFame, progressBarTimer, skipButton;
        [SerializeField] private TextMeshPro fameValueText;
        private ProgressBar _progressBarFame, _progressBarTimer;
        private Camera _camera;
        private Animator _cameraAnimator;
        private bool _preventTouch;
        private CardInfo[] _cardsInfo;

        [NonSerialized] public float SkipTimeMultiplier;

        private void Awake()
        {
            _camera = Camera.main;
            _cameraAnimator = _camera!.GetComponent<Animator>();
            (_progressBarFame, _progressBarTimer) = (progressBarFame.GetComponent<ProgressBar>(), progressBarTimer.GetComponent<ProgressBar>());
            SkipTimeMultiplier = 1;
        }

        private void Start()
        {
            _cardsInfo = cardsInfoPlaceholder.GetComponentsInChildren<CardInfo>();
        }

        private void FixedUpdate()
        {
            TouchHandler();
        }

        private void TouchHandler()
        {
            if (_preventTouch || !(Input.touchCount > 0)) return;
            var layer = 1 << LayerMask.NameToLayer("UI");
            
            var touch = Input.GetTouch(0);
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    SkipButtonHandler(touch, layer);
                    break;
                case TouchPhase.Moved:
                    ScrollInfoCardsHandler(touch, layer);
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SkipButtonHandler(Touch touchEvent, int layer)
        {
            if (!Physics.Raycast(
                    _camera.ScreenPointToRay(touchEvent.position), 
                    out var hit, 
                    float.PositiveInfinity,
                     layer) 
                || hit.transform.gameObject != skipButton) return;
            
            SkipTimeMultiplier = 15;
            _cameraAnimator.speed = SkipTimeMultiplier;
            _preventTouch = true;
        }

        private void ScrollInfoCardsHandler(Touch touchEvent, int layer)
        {
            if (!(_cardsInfo.Length > 3)) return;
            if (!Physics.Raycast(_camera.ScreenPointToRay(touchEvent.position), out var hit, float.PositiveInfinity, layer)) return;
            if (hit.transform.gameObject != cardsInfoPlaceholder.gameObject) return;

            if (_cardsInfo[0].transform.localPosition.x > 0 && touchEvent.deltaPosition.x > 0) return;
            if(_cardsInfo[^1].transform.localPosition.x <= 0 && touchEvent.deltaPosition.x <= 0) return;
            foreach (var cardInfo in _cardsInfo)
            {
                cardInfo.transform.localPosition += Vector3.right * touchEvent.deltaPosition.x / 1000;
            }
        }

        public void ChangeFameValueText(int value)
        {
            fameValueText.text = value.ToString();
        }

        public void SetCurrentFameProgressValue(float currentFame)
        {
            _progressBarFame.PositionManagement(currentFame, 150);
        }
        
        public void SetCurrentTimerProgressValue(float currentTime)
        {
            var linearInterpolation = _progressBarTimer.PositionManagement(currentTime, 40, true);
            _progressBarTimer.ColorManagement(linearInterpolation);
        }
    }
}