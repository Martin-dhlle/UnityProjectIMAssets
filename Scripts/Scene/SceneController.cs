using System;
using UnityEngine;

namespace Scene
{
    public class SceneController : MonoBehaviour
    {
        public Transform coordinatesToEndIntro;
        
        private IntroductionManager _introductionManager;
        private StageManager _stageManager;
        
        public enum ScenePhaseEnum {Introduction, Preparation, Stage1, Stage2, BadEnd, HappyEnd}
        [SerializeField] private ScenePhaseEnum initialScenePhase;

        private void Awake()
        {
            _introductionManager = GetComponent<IntroductionManager>();
            _stageManager = GetComponent<StageManager>();
        }

        /// <summary>
        /// At the start of the game, we set the scene state to the initial scene state defined
        /// in unity inspector
        /// </summary>
        private void Start()
        {
            SwitchSceneState(initialScenePhase);
        }

        public void SwitchSceneState(ScenePhaseEnum state)
        {
            switch (state)
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
                    _stageManager.enabled = false;
                    // _preparationManager.enabled = true;
                    // dont forget to set the main camera Animator "apply root motion" boolean to false
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
