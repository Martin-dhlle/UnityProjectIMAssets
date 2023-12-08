using System;
using TMPro;
using UI.Elements.ProgressBar;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour, ICard
    {
        [SerializeField] private TMP_Text textForce, textType, textForceSlider;
        [SerializeField] private GameObject battleStack, editStack;
        [SerializeField] private GameObject slider;
        private ProgressBar _slider;
        
        [SerializeField] private Texture MAIN_Slash, MAIN_Thrust, MAIN_Bash;
        private Renderer _rend;
        private static readonly int Main = Shader.PropertyToID("_MainTex");

        public int Force { get; private set; }
        public ICard.TypeEnum Type { get; private set; }
        
        private void Awake()
        {
            _slider = slider.GetComponent<ProgressBar>();
            _rend = GetComponentInChildren<Renderer>();
        }

        /// <summary>
        /// Add force to the card
        /// </summary>
        /// <param name="force"></param>
        /// <param name="isInitialAdd"></param>
        public void AddForce(int force, bool isInitialAdd = false)
        {
            Force += force;
            if (isInitialAdd) return;
            textForceSlider.text = Force.ToString();
            _slider.PositionManagement(Force, 150);
        }

        /// <summary>
        /// Change the type of the card and update instantly the text
        /// </summary>
        /// <param name="type"></param>
        public void ChangeType(ICard.TypeEnum type)
        {
            Type = type;
            textType.text = $"Type : {Type.ToString()}";
            switch (type)
            {
                case ICard.TypeEnum.Bash:
                    _rend.material.SetTexture(Main, MAIN_Bash);
                    break;
                case ICard.TypeEnum.Slash:
                    _rend.material.SetTexture(Main, MAIN_Slash);
                    break;
                case ICard.TypeEnum.Thrust:
                    _rend.material.SetTexture(Main, MAIN_Thrust);
                    break;
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// Update the force value text on the card
        /// </summary>
        public void UpdateText()
        {
            textForce.text = $"Force : {Force.ToString()}";
        }

        /// <summary>
        /// Change the current mode of the card and refresh the visual of the card
        /// </summary>
        /// <param name="mode"></param>
        public void SetCardMode(ICard.ModeEnum mode)
        {
            switch (mode)
            {
                case ICard.ModeEnum.Battle:
                    textForce.text = $"Force : {Force.ToString()}";
                    textType.text = $"Type : {Type.ToString()}";
                    editStack.SetActive(false);
                    battleStack.SetActive(true);
                    break;
                case ICard.ModeEnum.Edit:
                    textForceSlider.text = Force.ToString();
                    battleStack.SetActive(false);
                    editStack.SetActive(true);
                    _slider.PositionManagement(Force, 150);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}
