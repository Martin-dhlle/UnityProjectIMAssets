using System;
using System.Collections;
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
        private float _timer;
        private bool _canPassRound;
        
        private void Awake()
        {
            _camera = Camera.main;
            _sceneController = GetComponent<SceneController>();
            _cardsStorage = GetComponent<CardsStorage>();
        }
        
        private void Start()
        {
            _battleGUIController = HelperScripts.UI.InstantiateGUI<BattleGUI>(battleGUI, _camera);
            Invoke(nameof(InitializeCards), 1);
        }

        private void Update()
        {
            if (_canPassRound)
            {
                StartARound();
            }
            WatchRoundState();
        }

        private void InitializeCards()
        {
            _cardsStorage.SpawnAllCardsInScene(_battleGUIController.placeholder.position);
            _canPassRound = true;
        }

        private void WatchRoundState()
        {
            if (!_canPassRound) return;
            _canPassRound = false;
        }

        private void StartARound()
        {
            StartCoroutine(_cardsStorage.AnimateManyAsync(CardsStorage.AnimationTypeEnum.Spawn, 1));
        }

        private void CardTouchHandler()
        {
            
        }
    }
}