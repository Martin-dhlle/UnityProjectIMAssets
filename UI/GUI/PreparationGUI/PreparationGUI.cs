using System;
using Cards;
using TMPro;
using UI.Elements.ProgressBar;
using UnityEngine;

namespace UI.GUI.PreparationGUI
{
    public class PreparationGUI: MonoBehaviour
    {
        [SerializeField] private GameObject progressBarFame, thrustSlider, slashSlider, bashSlider;
        private ProgressBar _progressBarFame, _thrustSlider, _slashSlider, _bashSlider;
        [SerializeField] private TextMeshPro fameValueText;

        private void Awake()
        {
            _progressBarFame = progressBarFame.GetComponent<ProgressBar>();
        }

        public void ChangeFameValueText(int value)
        {
            fameValueText.text = value.ToString();
        }

        public void SetCurrentFameProgressValue(float currentFame)
        {
            _progressBarFame.PositionManagement(currentFame, 150);
        }

        public void ChangeSliderValue(ICard.TypeEnum sliderType, float value)
        {
            switch (sliderType)
            {
                case ICard.TypeEnum.Thrust:
                    _thrustSlider.PositionManagement(value, 150);
                    break;
                case ICard.TypeEnum.Slash:
                    _slashSlider.PositionManagement(value, 150);
                    break;
                case ICard.TypeEnum.Bash:
                    _bashSlider.PositionManagement(value, 150);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sliderType), sliderType, null);
            }
        }
    }
}