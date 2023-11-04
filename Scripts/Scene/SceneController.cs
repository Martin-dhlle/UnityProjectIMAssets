using System;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject introductionCamera, preparationCamera, battleCamera;
    public GameObject bout1, bout2; // they need to be disabled in the scene before use
    public BoutController bout1Controller, bout2Controller;

    private enum SceneStateEnum {Introduction, Preparation, Bout1, Bout2, BadEnd, HappyEnd}
    private SceneStateEnum _sceneState = SceneStateEnum.Introduction;
    private bool _canSwitchState;

    private void Awake()
    {
        bout1Controller = bout1.GetComponent<BoutController>();
        bout2Controller = bout2.GetComponent<BoutController>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        ManageLostAndVictory();
        if (_canSwitchState) SwitchState();
    }

    /**
     * -- TEST --
     * Remove this method in the future by replacing the logic of the code
     * with the monster state to decide victory or lost (health ect...)
     */
    private void ManageLostAndVictory()
    {
        // test code.
        // in the future, replace all this code with dragon state, it will more simple.
        if (bout1Controller.boutLost)
        {
            _sceneState = SceneStateEnum.Preparation;
            _canSwitchState = true;
        }

        if (bout2Controller.boutLost)
        {
            _sceneState = SceneStateEnum.BadEnd;
            _canSwitchState = true;
        }

        if (bout1Controller.boutVictory || bout2Controller.boutVictory)
        {
            _sceneState = SceneStateEnum.HappyEnd;
            _canSwitchState = true;
        }
    }

    private void SwitchState()
    {
        /*
         * bout state cycle in the explained :
         * Introduction => introduction of the game, camera moving around castle stuff...
         * --(Preparation state to put here before battle)--
         * Bout1 => first bout
         * Preparation => state switch switch to Preparation
         * Bout2 => second bout
         * BadEnd => if dragon arrive at the castle, bad end...
         * HappyEnd => if dragon is gone before arriving at the castle, happy end !
         */
        switch (_sceneState)
        {
            case SceneStateEnum.Introduction:
                // some introduction stuff
                break;
            case SceneStateEnum.Preparation:
                bout1.SetActive(false);
                bout2.SetActive(false);
                // some preparation stuff
                break;
            case SceneStateEnum.Bout1:
                bout1.SetActive(true);
                break;
            case SceneStateEnum.Bout2:
                bout1.SetActive(false);
                bout2.SetActive(true);
                break;
            case SceneStateEnum.BadEnd:
                // some bad ending stuff
                break;
            case SceneStateEnum.HappyEnd:
            // HAPPY ENDING !
            default:
                throw new NotImplementedException();
        }

        _canSwitchState = false;
    }
}
