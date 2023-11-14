using System;
using UnityEngine;

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
        private void Start()
        {
            SwitchScenePhase(initialScenePhase);
        }

        public void SwitchScenePhase(ScenePhaseEnum phase)
        {
            Debug.Log($"current phase{phase}");
            switch (phase)
            {
                case ScenePhaseEnum.Introduction:
                    _introductionManager.enabled = true;
                    // some introduction stuff
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
                    break;
                case ScenePhaseEnum.BadEnd:
                    // some bad ending stuff
                    break;
                case ScenePhaseEnum.HappyEnd:
                // HAPPY ENDING !
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
