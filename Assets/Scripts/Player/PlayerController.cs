using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BonesVr.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Scene-Singleton

        private static Dictionary<Scene, PlayerController> SceneSingletons { get; set; } = new();

        public static PlayerController SceneInstance(Scene scene)
        {
            if (SceneSingletons.TryGetValue(scene, out PlayerController playerController))
                return playerController;
            else
            {
                PlayerController player = null;

                foreach (GameObject root in scene.GetRootGameObjects())
                {
                    player = root.GetComponentInChildren<PlayerController>();
                    if (player != null)
                        break;
                }

                if (player != null)
                {
                    SceneSingletons.Add(scene, player);
                    return player;
                }
                else
                {
                    Debug.LogError("No player controller found in scene");
                    return null;
                }
            }
        }

        public static PlayerController SceneInstance(GameObject gameObject) => SceneInstance(gameObject.scene);

        #endregion

        public void SetPlayerPosition(Vector3 pos) => transform.position = pos;
        public void SetPlayerRotation(Quaternion rot) => transform.rotation = rot;
    }
}
