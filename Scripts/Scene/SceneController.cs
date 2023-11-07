using System;
using UI.GUI.IntroductionGUI;
using UnityEngine;

namespace Scene
{
    public class SceneController : MonoBehaviour
    {
        private IntroductionManager _introductionManager;
        private StageManager _stageManager;
        
        public enum SceneStateEnum {Introduction, Preparation, Stage1, Stage2, BadEnd, HappyEnd}
        public SceneStateEnum sceneState = SceneStateEnum.Introduction;
        public bool canSwitchState;

        private void Awake()
        {
            _introductionManager = GetComponent<IntroductionManager>();
            _stageManager = GetComponent<StageManager>();
        }

        private void Start()
        {
            canSwitchState = true;
        }

        private void Update()
        {
            if(canSwitchState) SwitchState();
        }
        
        private void SwitchState()
        {
            switch (sceneState)
            {
                case SceneStateEnum.Introduction:
                    _introductionManager.enabled = true;
                    // some introduction stuff
                    break;
                case SceneStateEnum.Stage1:
                    _introductionManager.enabled = false;
                    _stageManager.enabled = true;
                    break;
                case SceneStateEnum.Preparation:
                    break;
                case SceneStateEnum.Stage2:
                    break;
                case SceneStateEnum.BadEnd:
                    // some bad ending stuff
                    break;
                case SceneStateEnum.HappyEnd:
                // HAPPY ENDING !
                default:
                    throw new NotImplementedException();
            }

            canSwitchState = false;
        }
    }
}
