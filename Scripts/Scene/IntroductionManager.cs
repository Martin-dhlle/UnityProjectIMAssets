using UI.GUI.IntroductionGUI;
using UnityEngine;

namespace Scene
{
    public class IntroductionManager: MonoBehaviour
    {
        private SceneController _sceneController;
        
        public GameObject introductionGUI;
        public Transform coordinatesToEndIntro;
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
            _introductionGUIController.CoordinatesToEndIntro = coordinatesToEndIntro;
        }

        private void Update()
        {
            CheckIntroductionState();
        }

        private void CheckIntroductionState()
        {
            if (!_introductionGUIController.isIntroOver) return;
            _sceneController.canSwitchState = true;
            _sceneController.sceneState++;
        }
    }
}