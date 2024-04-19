using BonesVr.Progression;
using UnityEngine;

namespace BonesVr.SceneAreaStageControllers
{
    public abstract class SceneAreaStageController : MonoBehaviour
    {
        [SerializeField] private PlayerProgress _playerProgress;
        protected PlayerProgress PlayerProgress => _playerProgress;

        protected virtual void Awake()
        {
            if (PlayerProgress == null)
                Debug.LogError("No player progress instance provided");
        }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        public void ProgressFromStage()
        {
            PlayerProgress.ProgressStage();
        }
    }
}
