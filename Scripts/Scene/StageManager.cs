using Monster;
using UI.GUI.BattleGUI;
using UnityEngine;

namespace Scene
{
    public class StageManager: MonoBehaviour
    {
        private SceneController _sceneController;
        private CardsStorage _cardsStorage;
        
        [SerializeField] private GameObject battleGUI, monster;
        private BattleGUI _battleGUIController;
        private MonsterController _monsterController;
        
        private Camera _camera;
        private float _timer;
        private int _round, _stage;
        
        private void Awake()
        {
            _camera = Camera.main;
            _sceneController = GetComponent<SceneController>();
            _cardsStorage = GetComponent<CardsStorage>();
            _monsterController = monster.GetComponent<MonsterController>();
        }
        
        /// <summary>
        /// At the start of the first stage, force the camera to be at the coordinatesToEndIntro position,
        /// then invoke after 2 seconds the InitializeStage method.
        /// </summary>
        private void Start()
        {
            var cameraTransform = _camera.transform;
            cameraTransform.position = _sceneController.coordinatesToEndIntro.position;
            cameraTransform.rotation = _sceneController.coordinatesToEndIntro.rotation;
            Invoke(nameof(InitializeStage), 2);
        }

        private void FixedUpdate()
        {
            CardTouchHandler();
        }

        /// <summary>
        /// Initialize the stage by instantiate the UI
        /// and spawn all cards at the cards placeholder position.
        /// </summary>
        private void InitializeStage()
        {
            _battleGUIController = HelperScripts.UI.InstantiateGUI<BattleGUI>(battleGUI, _camera);
            _cardsStorage.SpawnAllCardsInScene(_battleGUIController.placeholder.position);
            StartNextRound();
        }

        /// <summary>
        /// Start the current round by increment the number of count and animate all cards.
        /// If the round count is more than 10 then switch to preparation phase.
        /// Get the qte value and 
        /// </summary>
        private void StartNextRound()
        {
            _round++;
            if(_round > 10) _sceneController.SwitchSceneState(SceneController.ScenePhaseEnum.Preparation);

            var qte = _monsterController.GetQte(_round);
            Debug.Log(qte);
            
            StartCoroutine(_cardsStorage.AnimateManyAsync(CardsStorage.AnimationTypeEnum.Spawn, 1, 2));
            _battleGUIController.StartQte(qte);
        }
        
        /// <summary>
        /// In every fixed update, detect if there is a touch on the screen and
        /// do a action.
        /// </summary>
        private void CardTouchHandler()
        {
            if (!(Input.touchCount > 0)) return;
            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began) return;
            
            var layer = 1 << LayerMask.NameToLayer("Card");
            if (!Physics.Raycast(_camera.ScreenPointToRay(touch.position), out var hit, layer)) return;
            
            var card = hit.transform.gameObject;
            
            _cardsStorage.AddForceToSingleCard(card,10);
            
            // _cardsStorage.AnimateSingle(card, CardsStorage.SingleAnimationTypeEnum.Focus);
            /*
             * if(_cardsStorage.getType(card) != monster.getType() || _cardsStorage.getForce(card) < monster.GetForce())
             * {
             *  Put code to declare lose of this stage
             *  _cardsStorage.AnimateManyAsync(card, CardsStorage.SingleAnimationTypeEnum.);
             * }
             * battleGUI.timer = 0;
             * 
             */
        }
    }
}