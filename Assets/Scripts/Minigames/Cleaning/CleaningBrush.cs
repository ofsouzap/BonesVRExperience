using UnityEngine;

namespace BonesVr.Minigames.Cleaning
{
    [RequireComponent(typeof(Rigidbody))]
    public class CleaningBrush : MonoBehaviour
    {
        public Rigidbody Rigidbody => GetComponent<Rigidbody>();

        [Header("Cleaning Targeting")]

        [SerializeField] private CleaningBrushTriggerController _triggerController;
        protected CleaningBrushTriggerController TriggerController => _triggerController;

        [Header("Action Conditions")]

        [Tooltip("The minimum speed the brush must be moving at to perform a cleaning action")]
        [Min(0f)]
        [SerializeField] private float _minimumCleanSpeed;
        protected float MinimumCleanSpeed => _minimumCleanSpeed;
        protected float MinimumCleanSpeedSqr => MinimumCleanSpeed * MinimumCleanSpeed;

        [Tooltip("The minimum time to wait between performing the clean action")]
        [Min(0f)]
        [SerializeField] private float _cleanActionDelay;
        protected float CleanActionDelay => _cleanActionDelay;

        [Header("Clean Effect Configuration")]

        [Tooltip("How much to change the dirtiness of the target by on each successful clean")]
        [Range(0, 1)]
        [SerializeField] private float _cleanDirtinessChange;
        protected float CleanDirtinessChange => _cleanDirtinessChange;

        [Header("Cleaning FX")]

        [SerializeField] private GameObject _dustParticlesFxPrefab;
        protected GameObject DustParticlesFxPrefab => _dustParticlesFxPrefab;

        private float? m_LastCleanActionTime;

        protected virtual void Start()
        {
            m_LastCleanActionTime = null;
        }

        protected virtual void OnEnable()
        {
            TriggerController.DirtyBoneEnteredTrigger.AddListener(TryClean);
        }

        protected virtual void OnDisable()
        {
            TriggerController.DirtyBoneEnteredTrigger.RemoveListener(TryClean);
        }

        protected virtual void OnValidate()
        {
            if (!DustParticlesFxPrefab.TryGetComponent<DirtParticleSprayController>(out var _))
                Debug.LogError("Dust particles FX prefab has no dirt particle spray controller");
        }

        protected void TryClean(DirtyBoneController dirtyBone)
        {
            if (CheckCanClean())
                PerformCleanAction(dirtyBone);
        }

        protected bool CheckCanClean()
            => Rigidbody.velocity.sqrMagnitude >= MinimumCleanSpeedSqr
            && (!m_LastCleanActionTime.HasValue || Time.time >= m_LastCleanActionTime.Value + CleanActionDelay);

        protected void PerformCleanAction(DirtyBoneController dirtyBone)
        {
            m_LastCleanActionTime = Time.time;

            if (dirtyBone.GetDirtiness() > 0)
                SpawnCleaningFxPrefab(dirtyBone);

            dirtyBone.ChangeDirtiness(-CleanDirtinessChange);
        }

        protected void SpawnCleaningFxPrefab(DirtyBoneController dirtyBone)
        {
            GameObject spawned = Instantiate(DustParticlesFxPrefab, dirtyBone.transform.position, dirtyBone.transform.rotation);
            if (spawned.TryGetComponent<DirtParticleSprayController>(out var particlesController))
            {
                particlesController.SetConfig(new DirtParticleSprayController.Config());
            }
            else
                Debug.LogWarning("No dirt particle spary controller");
        }
    }
}
