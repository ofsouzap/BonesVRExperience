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

            UpdateStagePrefabs();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            PlayerProgress.OnStageChanged.AddListener(UpdateStagePrefabs);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            PlayerProgress.OnStageChanged.RemoveListener(UpdateStagePrefabs);
        }

        protected void UpdateStagePrefabs()
        {
            if (PlayerProgress.GetCurrentStage().GetMainPlatformStagePrefab() != null)
                SetMainPlatformStagePrefab(PlayerProgress.GetCurrentStage().GetMainPlatformStagePrefab());
        }

        protected void SetMainPlatformStagePrefab(GameObject prefab)
        {
            if (m_MainPlatformCurrStageTransform != null)
                Destroy(m_MainPlatformCurrStageTransform.gameObject);

            m_MainPlatformCurrStageTransform = Instantiate(prefab, MainPlatformRoot).transform;
        }
    }
}
