using UnityEngine;

namespace BonesVr.SceneControllers
{
    public abstract class SceneController : MonoBehaviour
    {
        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
    }
}
