using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BonesVr.Utils
{
    public static class SceneSingleton<T> where T : MonoBehaviour
    {
        #region Scene-Singleton

        private static Dictionary<Scene, T> SceneSingletons { get; set; } = new();

        /// <summary>
        /// Find the instance of a MonoBehvaiour in a scene. This will return an instance if there is one in the scene.
        /// </summary>
        public static T SceneInstance(Scene scene)
        {
            if (SceneSingletons.TryGetValue(scene, out T cachedInstance))
                return cachedInstance;
            else
            {
                bool instanceFound = false;
                T instance = default;

                foreach (GameObject root in scene.GetRootGameObjects())
                {
                    instance = root.GetComponentInChildren<T>();
                    if (instance != null)
                    {
                        instanceFound = true;
                        break;
                    }
                }

                if (instanceFound)
                {
                    SceneSingletons.Add(scene, instance);
                    return instance;
                }
                else
                {
                    Debug.LogError("No instance found in scene");
                    return default;
                }
            }
        }

        public static T SceneInstance(GameObject gameObject) => SceneInstance(gameObject.scene);

        #endregion
    }
}
