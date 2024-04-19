using BonesVr.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BonesVr.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController SceneInstance(GameObject gameObject) => SceneSingleton<PlayerController>.SceneInstance(gameObject.scene);

        public void SetPlayerPosition(Vector3 pos) => transform.position = pos;
        public void SetPlayerRotation(Quaternion rot) => transform.rotation = rot;
    }
}
