using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Json;
using Newtonsoft.Json;
using UnityEngine;

namespace Bout
{
    public class BoutController : MonoBehaviour
    {
        public TextAsset patternJson;
        public GameObject cardsControllerObject, cardPrefab, battleCamera;

        private Card _cardPrefabData;
        private CardsController _cardsController;
        private GameObject _cardsControllerInstance;
        private Dictionary<int, List<GameObject>> _allCardsPattern = new();
    
        public int boutNumber;
        public bool boutLost, boutVictory, canPassRound;
        public float cameraVelocity;
        
        private Camera _camera;
        private Vector3 _camStartPos;
        
        private int _roundCount;

        private enum BoutStateEnum
        {
            Introduction,
            Battle
        }

        private BoutStateEnum _boutState;

        /// <summary>
        /// Define the main camera who has MainCamera tag
        /// </summary>
        private void Awake()
        {
            _camera = battleCamera.GetComponent<Camera>();
        }

        /// <summary>
        /// At the start of the bout, define the start position of the main camera
        /// and pass cards from the preparation phase to the cards controller.
        /// </summary>
        private void Start()
        {
            DefineMonsterCardsPattern();
            _camStartPos = _camera.transform.position;
        }

        private void Update()
        {
            if (_boutState == BoutStateEnum.Introduction)
            {
                MoveUntilCoordinate(transform.position);
            }
        
            if (canPassRound && _boutState == BoutStateEnum.Battle)
            {
                StartRound();
            }
        }
        
        /// <summary>
        /// Get all pattern data from JSON files attached to BoutController object and attribute these data to each commands.
        /// </summary>
        private void DefineMonsterCardsPattern()
        {
            cardPrefab.SetActive(false);
            _cardPrefabData = cardPrefab.GetComponent<Card>();
            var patterns = JsonConvert.DeserializeObject<JsonPattern>(patternJson.text);
            foreach (var pattern in patterns.Patterns)
            {
                var list = new List<GameObject>();
                foreach (var cardData in pattern.Value)
                {
                    if (!Enum.TryParse<ICard.TypeEnum>(cardData.Type, out var type)) return;
                    (_cardPrefabData.CardName, _cardPrefabData.Force, _cardPrefabData.Type) 
                        = (cardData.CardName, cardData.Force, type);
                    list.Add(cardPrefab);
                }
                
                _allCardsPattern.Add(int.Parse(pattern.Key), list);
            }
            
            var cardsControllerInstance = Instantiate(cardsControllerObject, transform.position + new Vector3(0,-1f,3) , Quaternion.identity);
            _cardsController = cardsControllerInstance.GetComponent<CardsController>();
        }

        /// <summary>
        /// Translate the camera with his Z axis (forward) until it reach the Scene Controller coordinate.
        /// </summary>
        private void MoveUntilCoordinate(Vector3 coordinateToReach)
        {
            var cameraPosition = _camera.transform.position;
            var lerpDistanceFactor = (1 - Mathf.InverseLerp(_camStartPos.z, coordinateToReach.z, cameraPosition.z)) + 0.20f;
            var newCurrentCameraZPosition = cameraPosition.z + cameraVelocity * lerpDistanceFactor * Time.deltaTime;
        
            var clampedCoordinateZ = Mathf.Clamp(
                newCurrentCameraZPosition,
                _camStartPos.z,
                coordinateToReach.z
            );
        
            cameraPosition = new Vector3(coordinateToReach.x, coordinateToReach.y, clampedCoordinateZ);
            _camera.transform.position = cameraPosition;
            if (Math.Abs(coordinateToReach.z - clampedCoordinateZ) < 0.1) _boutState = BoutStateEnum.Battle;
        }

        /// <summary>
        /// Increment the round
        /// </summary>
        private void StartRound()
        {
            _roundCount++;
            // Define cardsPattern of the cards controller based on the roundCount value
            _cardsControllerInstance.SetActive(false);
            _cardsController.cardsPattern = _allCardsPattern[_roundCount];
            _cardsControllerInstance.SetActive(true);
            // Spawn cards
            StartCoroutine(_cardsController.SpawnCards());
            canPassRound = false;
        }
    }
}
