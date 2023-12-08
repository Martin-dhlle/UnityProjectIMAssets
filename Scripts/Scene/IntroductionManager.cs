using Monster;
using UI.GUI.IntroductionGUI;
using UnityEngine;

namespace Scene
{
    public class IntroductionManager: MonoBehaviour
    {
        private SceneController _sceneController;
        
        [SerializeField] private GameObject introductionGUI, monster;
        private MonsterController _monsterController;
        
        private IntroductionGUI _introductionGUIController;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _monsterController = monster.GetComponent<MonsterController>();
            _sceneController = GetComponent<SceneController>();
        }

        private void Start()
        {
            _introductionGUIController = HelperScripts.UI.InstantiateGUI<IntroductionGUI>(introductionGUI, _camera);
            _introductionGUIController.CoordinatesToEndIntro = _sceneController.coordinatesToEndIntro;
            Invoke(nameof(AnimateMonsterMeow), 15);
        }

        private void Update()
        {
            CheckIntroductionState();
        }

        private void CheckIntroductionState()
        {
            if (!_introductionGUIController.isIntroOver) return;
            _sceneController.SwitchScenePhase(SceneController.ScenePhaseEnum.Stage1);
        }

        private void AnimateMonsterMeow()
        {
            Debug.Log("MEEOOOWWW !!!!");
            _monsterController.Animate(MonsterController.MonsterAnimationEnum.Meow);
        }
    }
}