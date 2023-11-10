using System.Collections;
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
        private int _round, _stage = 1;
        
        private void Awake()
        {
            _camera = Camera.main;
            _sceneController = GetComponent<SceneController>();
            _cardsStorage = GetComponent<CardsStorage>();
            _monsterController = monster.GetComponent<MonsterController>();
            _battleGUIController = battleGUI.GetComponent<BattleGUI>();
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
            Invoke(nameof(InitializeStage), 1);
        }

        /// <summary>
        /// Watch the qte bool value from progressBar.
        /// If it's true, then switch to the next stage.
        /// </summary>
        private void Update()
        {
            if (!_battleGUIController.qteIsOver) return;
            StartCoroutine(StartNextStage());
            _battleGUIController.qteIsOver = false;
        }

        private void FixedUpdate()
        {
            CardTouchHandler();
        }

        /// <summary>
        /// At the start of the first stage, initialize the stage by instantiate
        /// he UI and spawn all cards at the cards placeholder position.
        /// </summary>
        private void InitializeStage()
        {
            _battleGUIController = HelperScripts.UI.InstantiateGUI<BattleGUI>(battleGUI, _camera);
            _battleGUIController.DisableQteProgressBar();
            _cardsStorage.SpawnAllCardsInScene(_battleGUIController.placeholder.position);
            Invoke(nameof(StartNextRound), 1);
        }
        
        /// <summary>
        /// In every fixed update, detect if there is a touch on the screen and on a card.
        /// Then, do a action.
        /// </summary>
        private void CardTouchHandler()
        {
            if (!(Input.touchCount > 0)) return;
            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began) return;
            
            var layer = 1 << LayerMask.NameToLayer("Card");
            if (!Physics.Raycast(_camera.ScreenPointToRay(touch.position), out var hit, layer)) return;
            
            var card = hit.transform.parent.gameObject;
            
            _cardsStorage.AnimateSingle(card, CardsStorage.AnimationTypeEnum.Focus);
            
            // _cardsStorage.AnimateSingle(card, CardsStorage.SingleAnimationTypeEnum.Focus);
            /*
             * if(_cardsStorage.getType(card) != monster.getType() || _cardsStorage.getForce(card) < monster.GetForce())
             * {
             *  StartNextStage()
             * }
             * 
             */
        }
        
        /// <summary>
        /// Start the current round by increment the number of count and animate all cards.
        /// If the round count is more than 10 then switch to Victory phase.
        /// Get the qte value and 
        /// </summary>
        private void StartNextRound()
        {
            _round++;
            if(_round > 10) _sceneController.SwitchSceneState(SceneController.ScenePhaseEnum.HappyEnd);
            
            var qte = _monsterController.GetQte(_round);
            
            StartCoroutine(_cardsStorage.AnimateManyAsync(CardsStorage.AnimationTypeEnum.Spawn, 0.2f));
            StartCoroutine(_battleGUIController.StartQte(qte, 1));
        }
        
        /// <summary>
        /// Reset cards animation and some variables of this stage and decide if the game end
        /// or if the preparation can begin
        /// </summary>
        private IEnumerator StartNextStage()
        {
            _battleGUIController.DisableQteProgressBar();
            _cardsStorage.AnimateMany(CardsStorage.AnimationTypeEnum.Disappear);
            _battleGUIController.gameObject.SetActive(false);
            _sceneController.SwitchSceneState(_stage > 2 ? SceneController.ScenePhaseEnum.BadEnd : SceneController.ScenePhaseEnum.Preparation);
            yield return null;
        }
    }
}