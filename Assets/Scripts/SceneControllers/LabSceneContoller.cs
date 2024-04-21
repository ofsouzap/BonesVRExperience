using BonesVr.Player;
using BonesVr.Progression;
using BonesVr.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BonesVr.SceneControllers
{
    public class LabSceneContoller : SceneController
    {
        public static LabSceneContoller SceneInstance(GameObject gameObject) => SceneSingleton<LabSceneContoller>.SceneInstance(gameObject.scene);

        [SerializeField] private PlayerProgress _playerProgress;
        protected PlayerProgress PlayerProgress => _playerProgress;

        [Tooltip("The root transform for the main platform. This is used as a parent transform when instantiating stage prefabs")]
        [SerializeField] private Transform _mainPlatformRoot;
        protected Transform MainPlatformRoot => _mainPlatformRoot;

        private Scene? m_CurrStageSceneAdditive;

        protected override void Awake()
        {
            base.Awake();

            m_CurrStageSceneAdditive = null;
        }

        protected override void Start()
        {
            base.Start();

            LoadPlayerProgressStage(PlayerProgress.GetCurrentStage());
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            PlayerProgress.OnStageChanged.AddListener(OnPlayerProgressStageChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            PlayerProgress.OnStageChanged.RemoveListener(OnPlayerProgressStageChanged);
        }

        private void OnPlayerProgressStageChanged()
            => LoadPlayerProgressStage(PlayerProgress.GetCurrentStage());

        protected void LoadPlayerProgressStage(ProgressionStage stage)
        {
            Vector3? startPos = stage.GetPlayerStartPosition();
            if (startPos.HasValue)
                PlayerController.SceneInstance(gameObject).SetPlayerPosition(startPos.Value);

            Quaternion? startRot = stage.GetPlayerStartRotation();
            if (startRot.HasValue)
                PlayerController.SceneInstance(gameObject).SetPlayerRotation(startRot.Value);

            if (!string.IsNullOrEmpty(stage.GetStageSceneAdditiveName()))
                SetStageSceneAdditive(stage.GetStageSceneAdditiveName());
        }

        protected void SetStageSceneAdditive(string sceneAdditiveName)
        {
            if (m_CurrStageSceneAdditive.HasValue)
                SceneManager.UnloadSceneAsync(m_CurrStageSceneAdditive.Value);

            m_CurrStageSceneAdditive = SceneManager.LoadScene(sceneAdditiveName, new LoadSceneParameters(LoadSceneMode.Additive));
        }
    }
}
