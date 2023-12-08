using System;
using System.Collections;
using Cards;
using Monster;
using UI.GUI.PreparationGUI;
using UnityEngine;

namespace Scene
{
    public class PreparationManager: MonoBehaviour
    {
        [SerializeField] private float baseTimeVelocity = 1;

        private SceneController _sceneController;
        private CardsStorage _cardsStorage;
        private StageManager _stageManager;
        
        [SerializeField] private GameObject preparationGUI, monster, cardInfo;
        private PreparationGUI _preparationGUI;
        private MonsterController _monsterController;
            
        private Animator _cameraAnimator;
        private static readonly int IsPreparation = Animator.StringToHash("isPreparation");
        private static readonly int State = Animator.StringToHash("animationState");
        private Camera _camera;
        private bool _preventTouch;

        private void Awake()
        {
            _sceneController = GetComponent<SceneController>();
            _cardsStorage = GetComponent<CardsStorage>();
            _stageManager = GetComponent<StageManager>();
            _camera = Camera.main;
            _cameraAnimator = _camera!.GetComponent<Animator>();
        }

        private void Start()
        {
            InitializeCamera();
            InstantiateAndInitializeGUI();
            InitializeInfoCards();
            StartCoroutine(StartAndManageTimer());
            StartCoroutine(MoveDragonAlongWithCamera());
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
            _cardsStorage.SpawnAllCardsInScene(_preparationGUI.cardsPlaceholder.position, 0.8f, ICard.ModeEnum.Edit);
            _cardsStorage.AnimateMany(CardsStorage.AnimationTypeEnum.Spawn);
        }
        
        private void InitializeInfoCards()
        {
            _monsterController = monster.GetComponent<MonsterController>();

            var incrementVector = 0f;
            foreach (var infoCardData in _monsterController.GetPatternInfos())
            {
                if (int.Parse(infoCardData.Key) > _stageManager.round) break;
                var currentCardObject = Instantiate(cardInfo, _preparationGUI.cardsInfoPlaceholder);
                currentCardObject.transform.localPosition += Vector3.right * incrementVector;
                var currentCardData = currentCardObject.GetComponent<CardInfo>();
                currentCardData.SetAttackName(infoCardData.Value.AttackName);
                currentCardData.SetForce(infoCardData.Value.Force);
                currentCardData.SetType(infoCardData.Value.Type);
                currentCardObject.GetComponentInChildren<Animator>().SetInteger(State,1);
                incrementVector += 0.7f;
            }
        }

        /// <summary>
        /// In the fixed update, detect if there is a touch with slide movement.
        /// Then increment or decrement cards slider along with the fame value.
        /// </summary>
        private void PlayerTouchOnInterfaceHandler()
        {
            if (!(Input.touchCount > 0)) return;
            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Moved) return;
            
            var layer = 1 << LayerMask.NameToLayer("Card");
            if (_preventTouch || !Physics.Raycast(_camera.ScreenPointToRay(touch.position), 
                    out var hit, float.PositiveInfinity, layer)) return;
            StartCoroutine(PreventTouch(0.5f));
            
            var card = hit.transform.parent.gameObject;
            switch(Mathf.Sign(touch.deltaPosition.y) > 0 ? float.PositiveInfinity : float.NegativeInfinity)
            {
                case float.PositiveInfinity:
                    if (_cardsStorage.AddPlayerFame(-10))
                    {
                        if (!_cardsStorage.AddForceToSingleCard(card.gameObject, 10))
                        {
                            _cardsStorage.AddPlayerFame(10);
                            return;
                        }
                        _preparationGUI.ChangeFameValueText(_cardsStorage.playerFame);
                        _preparationGUI.SetCurrentFameProgressValue(_cardsStorage.playerFame);
                    }
                    break;
                case float.NegativeInfinity:
                    if (_cardsStorage.AddPlayerFame(10))
                    {
                        if (!_cardsStorage.AddForceToSingleCard(card.gameObject, -10))
                        {
                            _cardsStorage.AddPlayerFame(-10);
                            return;
                        }
                        _preparationGUI.ChangeFameValueText(_cardsStorage.playerFame);
                        _preparationGUI.SetCurrentFameProgressValue(_cardsStorage.playerFame);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator PreventTouch(float seconds)
        {
            _preventTouch = true;
            yield return new WaitForSeconds(seconds);
            _preventTouch = false;
        }

        private IEnumerator StartAndManageTimer()
        {
            var currentTime = 0f;
            while (currentTime < 40)
            {
                currentTime += Time.deltaTime * baseTimeVelocity * _preparationGUI.SkipTimeMultiplier;
                _preparationGUI.SetCurrentTimerProgressValue(currentTime);
                yield return null;
            }
            _cardsStorage.AnimateMany(CardsStorage.AnimationTypeEnum.Disappear);
            yield return new WaitForSeconds(0.2f);
            _preparationGUI.gameObject.SetActive(false);
            _sceneController.SwitchScenePhase(SceneController.ScenePhaseEnum.Stage2);
        }

        private IEnumerator MoveDragonAlongWithCamera()
        {
            yield return new WaitForSeconds(2);
            monster.transform.parent = _camera.transform;
            // do monster walk animation
        }
    }
}