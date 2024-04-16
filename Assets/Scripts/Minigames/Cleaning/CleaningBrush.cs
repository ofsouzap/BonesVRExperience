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
            dirtyBone.ChangeDirtiness(-CleanDirtinessChange);
        }
    }
}
