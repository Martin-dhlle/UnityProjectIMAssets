using UI.GUI.BattleGUI;
using UnityEngine;

namespace Scene
{
    public class StageManager: MonoBehaviour
    {
        private SceneController _sceneController;
        private CardsStorage _cardsStorage;
        
        public GameObject battleGUI;
        
        private BattleGUI _battleGUIController;
        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
            _sceneController = GetComponent<SceneController>();
        }
        
        private void Start()
        {
            _battleGUIController = HelperScripts.UI.InstantiateGUI<BattleGUI>(battleGUI, _camera);
        }
    }
}