using BonesVr.Utils;
using UnityEngine;

namespace BonesVr.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Tooltip("The character controller for the player. If not set, will find it in the game object or its chlidren")]
        [SerializeField] private CharacterController _characterController;
        protected CharacterController CharacterController => _characterController;

        protected virtual void Awake()
        {
            if (_characterController == null)
                _characterController = GetComponentInChildren<CharacterController>();
            if (_characterController == null)
                Debug.LogError("No character controller set or found");
        }

        public static PlayerController SceneInstance(GameObject gameObject) => SceneSingleton<PlayerController>.SceneInstance(gameObject.scene);

        public void SetPlayerPosition(Vector3 pos)
        {
            // Need to temporarily disable character controller so it doesn't register a collision.
            CharacterController.enabled = false;
            transform.position = pos;
            CharacterController.enabled = true;
        }
        public void SetPlayerRotation(Quaternion rot)
        {
            // Need to temporarily disable character controller so it doesn't register a collision.
            CharacterController.enabled = false;
            transform.rotation = rot;
            CharacterController.enabled = true;
        }
    }
}
