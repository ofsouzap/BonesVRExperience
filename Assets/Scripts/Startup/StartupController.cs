using BonesVr.Progression;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BonesVr.Startup
{
    public class StartupController : MonoBehaviour
    {
        [SerializeField] private PlayerProgress _playerProgress;
        protected PlayerProgress PlayerProgress => _playerProgress;

        [SerializeField] private ProgressionStage _startingProgressionStage;
        protected ProgressionStage StartingProgressionStage => _startingProgressionStage;

        [SerializeField] private string _mainSceneName;
        protected string MainScenePath => _mainSceneName;

        protected virtual void Awake()
        {
            if (PlayerProgress == null)
                Debug.LogError("No player progress instance provided");

            if (StartingProgressionStage == null)
                Debug.LogError("No starting progression stage provided");
        }

        protected virtual void Start()
        {
            PlayerProgress.SetCurrentStage(StartingProgressionStage);

            SceneManager.LoadScene(MainScenePath);
        }
    }
}
