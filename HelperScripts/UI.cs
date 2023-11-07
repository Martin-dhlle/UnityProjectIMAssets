using UnityEngine;

namespace HelperScripts
{
    public static class UI
    {
        /// <summary>
        /// Instantiate GUI and define the position of the GUI scaled front of camera
        /// </summary>
        /// <param name="gameObjectGUI">The GUI game object to be instantiated</param>
        /// <param name="cameraFromScene">The camera in the scene</param>
        /// <typeparam name="T">The type of the GUI script</typeparam>
        /// <returns>The controller class of the GUI</returns>
        public static T InstantiateGUI<T>(GameObject gameObjectGUI, Camera cameraFromScene)
        {
            var instanceGUI = Object.Instantiate(gameObjectGUI, cameraFromScene.transform);
            instanceGUI.transform.localPosition += Vector3.forward * 5;
            return gameObjectGUI.GetComponent<T>();
        }
    }
}

