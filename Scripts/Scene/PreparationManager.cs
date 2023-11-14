using System;
using UI.GUI.PreparationGUI;
using UnityEngine;

namespace Scene
{
    public class PreparationManager: MonoBehaviour
    {
        private CardsStorage _cardsStorage;
        
        [SerializeField] private GameObject preparationGUI;
        private PreparationGUI _preparationGUI;
            
        private Camera _camera;
        private Animator _cameraAnimator;
        private static readonly int IsPreparation = Animator.StringToHash("isPreparation");

        private void Awake()
        {
            _camera = Camera.main;
            _cameraAnimator = _camera!.GetComponent<Animator>();
            _cardsStorage = GetComponent<CardsStorage>();
        }

        private void Start()
        {
            InstantiateAndInitializeGUI();
            InitializeCamera();
        }

        private void FixedUpdate()
        {
            // detect touch (swipe up and down on sliders) for the index 0 only
            PlayerTouchOnInterfaceHandler();
        }

        /// <summary>
        /// Set the camera animator to preparation and disable root motion
        /// </summary>
        private void InitializeCamera()
        {
            _cameraAnimator.SetBool(IsPreparation, true);
            _cameraAnimator.applyRootMotion = false;
        }

        /// <summary>
        /// Instantiate the GUI and initialize some values like the fame.
        /// </summary>
        private void InstantiateAndInitializeGUI()
        {
            _preparationGUI = HelperScripts.UI.InstantiateGUI<PreparationGUI>(preparationGUI, _camera);
            _preparationGUI.ChangeFameValueText(_cardsStorage.playerFame);
            _preparationGUI.SetCurrentFameProgressValue(_cardsStorage.playerFame);
            foreach (var card in _cardsStorage.GetAllCards())
            {
                _preparationGUI.ChangeSliderValue(card.Type, card.Force);
            }
        }

        private void PlayerTouchOnInterfaceHandler()
        {
            
        }
    }
}