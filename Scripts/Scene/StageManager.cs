using System.Collections;
using Firebase;
using Monster;
using UI.GUI.BattleGUI;
using UnityEngine;

namespace Scene
{
    /// <summary>
    /// All stages are managed here
    /// </summary>
    public class StageManager: MonoBehaviour
    {
        private SceneController _sceneController;
        private CardsStorage _cardsStorage;
        
        [SerializeField] private GameObject battleGUI, monster;
        private BattleGUI _battleGUIController;
        private MonsterController _monsterController;
        
        private Camera _camera;
        private float _timer;
        private bool _preventTouch;
        
        public int round, stage = 1;
        
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
        /// then invoke the InitializeStage method.
        /// </summary>
        private void Start()
        {
            var cameraTransform = _camera.transform;
            cameraTransform.position = _sceneController.coordinatesToEndIntro.position;
            cameraTransform.rotation = _sceneController.coordinatesToEndIntro.rotation;
            _battleGUIController = HelperScripts.UI.InstantiateGUI<BattleGUI>(battleGUI, _camera);
            StartCoroutine(InitializeNewStage());
        }

        /// <summary>
        /// Watch the qte bool value from progressBar.
        /// If qte is over, then switch to the next stage.
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
        /// Initialize the stage by instantiate the UI and spawn all cards at the cards placeholder position.
        /// Set the round state to 0 and start a new round
        /// </summary>
        public IEnumerator InitializeNewStage(bool randomPattern = false)
        {
            if(randomPattern) _monsterController.GenerateRandomPattern();
            round = 0;
            _battleGUIController.gameObject.SetActive(true);
            _battleGUIController.ShowAndFadeBackground();
            yield return new WaitForSeconds(1.5f);
            _cardsStorage.SpawnAllCardsInScene(_battleGUIController.placeholder.position);
            _monsterController.Animate(MonsterController.MonsterAnimationEnum.Meow);
            StartCoroutine(_monsterController.PlayAudio(MonsterController.AudioEnum.Win));
            StartCoroutine(StartNextRound());
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
            if (_preventTouch || !Physics.Raycast(_camera.ScreenPointToRay(touch.position), out var hit,
                       float.PositiveInfinity, layer)) return;
            
            var card = hit.transform.parent.gameObject;
            
            _preventTouch = true;
             if(!HelperScripts.CompareFunctions.CompareType(_cardsStorage.GetCardType(card), 
                    _monsterController.GetAttackType(round)) ||
                _cardsStorage.GetCardForce(card) < _monsterController.GetForce(round))
             {
                 _cardsStorage.AnimateSingle(card, CardsStorage.AnimationTypeEnum.MoveBottom);
                 StartCoroutine(_monsterController.PlayAudio(MonsterController.AudioEnum.Win));
                 StartCoroutine(StartNextStage());
                 return;
             }
             
            _cardsStorage.AnimateSingle(card, CardsStorage.AnimationTypeEnum.Focus);
            _cardsStorage.AddPlayerFame(_monsterController.GetFame(round));
            StartCoroutine(_monsterController.PlayAudio(MonsterController.AudioEnum.Lose));
            _monsterController.Animate(MonsterController.MonsterAnimationEnum.Default);
            StartCoroutine((StartNextRound()));
        }
        
        
        /// <summary>
        /// Start the current round by increment the number of count and animate all cards.
        /// If the round count is more than 10 then switch to Victory phase.
        /// Get the qte value and 
        /// </summary>
        private IEnumerator StartNextRound()
        {
            _battleGUIController.DisableQteProgressBar();
            round++;
            if(round > 10)
            {
                _sceneController.SwitchScenePhase(SceneController.ScenePhaseEnum.HappyEnd);
                yield break;
            }
            var qte = _monsterController.GetQte(round);
            _battleGUIController.WriteLog(round, _monsterController.GetAttackType(round), _monsterController.GetForce(round));
            yield return new WaitForSeconds(0.8f);
            _cardsStorage.AnimateMany(CardsStorage.AnimationTypeEnum.Disappear);
            _monsterController.AnimateAttack(_monsterController.GetAttackType(round), qte);
            yield return new WaitForSeconds(0.2f);
            _cardsStorage.DespawnAllCards();
            yield return new WaitForSeconds(0.5f);
            _cardsStorage.RespawnAllCards();
            _preventTouch = false;
            
            
            StartCoroutine(_cardsStorage.AnimateManyAsync(CardsStorage.AnimationTypeEnum.Spawn, 0.2f));
            _battleGUIController.StartQte(qte, 1);
        }
        
        /// <summary>
        /// Reset cards animation and some variables of this stage and decide if the game end
        /// or if the preparation can begin
        /// </summary>
        private IEnumerator StartNextStage()
        {
            var db = FirestoreDb.Init();
            db?.AddDataToHistory(stage, round);
            _monsterController.Animate(MonsterController.MonsterAnimationEnum.Move);
            yield return new WaitForSeconds(0.4f);
            _battleGUIController.DisableQteProgressBar();
            _cardsStorage.AnimateMany(CardsStorage.AnimationTypeEnum.Disappear);
            _battleGUIController.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _cardsStorage.DespawnAllCards();
            stage++;
            _sceneController.SwitchScenePhase(stage > 2 ? SceneController.ScenePhaseEnum.BadEnd : SceneController.ScenePhaseEnum.Preparation);
        }
    }
}