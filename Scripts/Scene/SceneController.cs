using System;
using System.Collections;
using System.Linq;
using Firebase;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Firestore;

namespace Scene
{
    /// <summary>
    /// The main controller of the game, manage active states of IntroductionManager, StageManager
    /// and PreparationManager
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        public Transform coordinatesToEndIntro;
        
        private IntroductionManager _introductionManager;
        private StageManager _stageManager;
        private PreparationManager _preparationManager;

        private FirebaseFirestore _firestore;
        
        public enum ScenePhaseEnum {Introduction, Preparation, Stage1, Stage2, BadEnd, HappyEnd}
        [SerializeField] private ScenePhaseEnum initialScenePhase;

        private void Awake()
        {
            _introductionManager = GetComponent<IntroductionManager>();
            _stageManager = GetComponent<StageManager>();
            _preparationManager = GetComponent<PreparationManager>();

            (_introductionManager.enabled, _stageManager.enabled, _preparationManager.enabled) 
                = (false, false, false);
        }

        /// <summary>
        /// At the start of the game, we set the scene state to the initial scene state defined
        /// in unity inspector
        /// </summary>
        private async void Start()
        {
            var db =  FirestoreDb.Init();
            db.ConnectFirebaseFirestore();
            var dataLength = (await db.GetHistoryDocumentsSnapshot()).Count();
            SwitchScenePhase(dataLength > 0 ? ScenePhaseEnum.Stage1 : initialScenePhase);
        }

        public void SwitchScenePhase(ScenePhaseEnum phase)
        {
            switch (phase)
            {
                case ScenePhaseEnum.Introduction:
                    _introductionManager.enabled = true;
                    break;
                case ScenePhaseEnum.Stage1:
                    _introductionManager.enabled = false;
                    _stageManager.enabled = true;
                    break;
                case ScenePhaseEnum.Preparation:
                    _preparationManager.enabled = true;
                    _stageManager.enabled = false;
                    break;
                case ScenePhaseEnum.Stage2:
                    _preparationManager.enabled = false;
                    _stageManager.enabled = true;
                    StartCoroutine(_stageManager.InitializeNewStage(true));
                    break;
                case ScenePhaseEnum.BadEnd:
                    StartCoroutine(RestartGame());
                    break;
                case ScenePhaseEnum.HappyEnd:
                // HAPPY ENDING !
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// --- TEST ---
        /// </summary>
        private static IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex);
        }
    }
}
