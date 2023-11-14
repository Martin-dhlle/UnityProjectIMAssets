using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.LEGACY_Json;
using Newtonsoft.Json;
using UnityEngine;

namespace LEGACY_Bout
{
    public class LegacyStageController : MonoBehaviour
    {
        public List<TextAsset> patternJson;
        public GameObject cardsControllerPrefab, cardPrefab;

        private LegacyCardsController _legacyCardsController;
        private GameObject _cardsControllerInstance;
        private readonly Dictionary<int, List<GameObject>> _allCardsPattern = new();
        private readonly Dictionary<GameObject, Card> _cardsToRetrieveFromObject = new();
    
        public int boutNumber;
        public bool boutLost, boutVictory, canStartNextRound;
        public float cameraVelocity;
        
        private Camera _camera;
        private Vector3 _camStartPos;
        
        private int _roundCount;

        private enum BoutStateEnum
        {
            Preparation,
            Battle,
        }

        private BoutStateEnum _boutState;
        
        private void Awake()
        {
            DefineMainCamera();
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
            DefineBoutState();
        }

        private void DefineBoutState()
        {
            switch (_boutState)
            {
                case BoutStateEnum.Preparation:
                    /* Show a new GUI to choose the Type */
                    // -- TEST -- //
                    canStartNextRound = true;
                    _boutState = BoutStateEnum.Battle;
                    // -- TEST -- //
                    break;
                case BoutStateEnum.Battle:
                    if (canStartNextRound)
                    {
                        StartNextRound();
                    }
                    break;
            }
        }

        /// <summary>
        /// Define the main camera who has "MainCamera" tag
        /// </summary>
        private void DefineMainCamera()
        {
            _camera = Camera.main;
        }
        
        /// <summary>
        /// Get all pattern data from JSON files attached to BoutController object and attribute these data to each commands.
        /// </summary>
        private void DefineMonsterCardsPattern()
        {
            cardPrefab.SetActive(false);
            var patterns = JsonConvert.DeserializeObject<JsonCardsPattern>(patternJson[0].text);
            foreach (var pattern in patterns.Patterns)
            {
                var list = new List<GameObject>();
                foreach (var cardData in pattern.Value)
                {
                    var cardInstance = Instantiate(cardPrefab);
                    var cardInstanceData = cardInstance.GetComponent<Card>();
                    if (!Enum.TryParse<ICard.TypeEnum>(cardData.Type, out var type)) return;
                    /*(cardInstanceData.Force, cardInstanceData.Type) 
                        = (cardData.Force, type);*/
                    _cardsToRetrieveFromObject.Add(cardInstance, cardInstanceData);
                    list.Add(cardInstance);
                    Destroy(cardInstance);
                }
                _allCardsPattern.Add(int.Parse(pattern.Key), list);
            }

            var cardsControllerInstance = Instantiate(cardsControllerPrefab, transform.position + new Vector3(0,-1f,3) , Quaternion.identity);
            _legacyCardsController = cardsControllerInstance.GetComponent<LegacyCardsController>();
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
            if (Math.Abs(coordinateToReach.z - clampedCoordinateZ) < 0.1) _boutState = BoutStateEnum.Preparation;
        }

        /// <summary>
        /// Increment the round
        /// </summary>
        private void StartNextRound()
        {
            _roundCount++;
            
            // Define cardsPattern of the cards controller based on the roundCount value and replace cards
            var replacementCardsData = _allCardsPattern[_roundCount].Select(cardObject => _cardsToRetrieveFromObject[cardObject]).ToList();
            StartCoroutine(_legacyCardsController.ChangeCards(replacementCardsData));
            canStartNextRound = false;
        }
    }
}
