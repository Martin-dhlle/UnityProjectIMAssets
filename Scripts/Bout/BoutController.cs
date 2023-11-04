using System;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace Bout
{
    public class BoutController : MonoBehaviour
    {
        public TextAsset patternJson;
        public List<GameObject> cardsPattern;
        public GameObject cardsControllerObject, battleCamera;
        private CardsController _cardsController;
    
        public int boutNumber;
        public bool boutLost, boutVictory;
        public float cameraVelocity;
        
        private Camera _camera;
        private Vector3 _camStartPos;

        private enum BoutStateEnum
        {
            Introduction,
            SpawnCards,
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
        
        
            // If the camera stop moving and introduction is over, so cards can begin to spawn.
            if (_boutState == BoutStateEnum.SpawnCards)
            {
                StartBattle();
            }
        
            if (_boutState == BoutStateEnum.Battle)
            {
            
            }
        }

        /**
     * Get all pattern data from JSON file attach to BoutController object and attribute these data to each commands.
     */
        private void DefineMonsterCardsPattern()
        {
            var pattern = JsonUtility.FromJson<ICard[]>(patternJson.text);
            foreach (var command in pattern)
            {
                Debug.Log(pattern);
            }
            var cardsControllerInstance = Instantiate(cardsControllerObject, transform.position + new Vector3(0,-1f,3) , Quaternion.identity);
            _cardsController = cardsControllerInstance.GetComponent<CardsController>();
            _cardsController.cardsPattern = cardsPattern;
            cardsControllerInstance.SetActive(true);
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
            if (Math.Abs(coordinateToReach.z - clampedCoordinateZ) < 0.1) _boutState = BoutStateEnum.SpawnCards;
        }

        /* private IEnumerator ShowIntroductionTextAndSpawnCards()
    {
        Debug.Log("5 seconds to wait...");
        yield return new WaitForSeconds(5);
        _cardsController.SpawnCards();
    } */

        private void StartBattle()
        {
            StartCoroutine(_cardsController.SpawnCards());
            _boutState = BoutStateEnum.Battle;
        }
    }
}
