using BonesVr.Player;
using BonesVr.Progression;
using UnityEngine;

namespace BonesVr.SceneControllers
{
    public class LabSceneContoller : SceneController
    {
        [SerializeField] private PlayerProgress _playerProgress;
        protected PlayerProgress PlayerProgress => _playerProgress;

        [Tooltip("The root transform for the main platform. This is used as a parent transform when instantiating scene area progression stage prefabs")]
        [SerializeField] private Transform _mainPlatformRoot;
        protected Transform MainPlatformRoot => _mainPlatformRoot;

        [SerializeField] private Transform m_MainPlatformCurrStageTransform;

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

            if (stage.GetMainPlatformStagePrefab() != null)
                SetMainPlatformStagePrefab(stage.GetMainPlatformStagePrefab());
        }

        protected void SetMainPlatformStagePrefab(GameObject prefab)
        {
            if (m_MainPlatformCurrStageTransform != null)
                Destroy(m_MainPlatformCurrStageTransform.gameObject);

            m_MainPlatformCurrStageTransform = Instantiate(prefab, MainPlatformRoot).transform;
        }
    }
}
