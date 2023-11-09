using System;
using UI.GUI.IntroductionGUI;
using UnityEngine;

namespace Scene
{
    public class IntroductionManager: MonoBehaviour
    {
        private SceneController _sceneController;
        
        [SerializeField] private GameObject introductionGUI;
        
        private IntroductionGUI _introductionGUIController;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _sceneController = GetComponent<SceneController>();
        }

        private void Start()
        {
            _introductionGUIController = HelperScripts.UI.InstantiateGUI<IntroductionGUI>(introductionGUI, _camera);
            _introductionGUIController.CoordinatesToEndIntro = _sceneController.coordinatesToEndIntro;
        }

        private void Update()
        {
            CheckIntroductionState();
        }

        private void CheckIntroductionState()
        {
            if (!_introductionGUIController.isIntroOver) return;
            _sceneController.SwitchSceneState(SceneController.ScenePhaseEnum.Stage1);
        }
    }
}