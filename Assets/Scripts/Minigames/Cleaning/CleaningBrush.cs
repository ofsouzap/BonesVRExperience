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

        [Tooltip("The maximum number of vertices to check when trying to approximate the UV coordinate of where to clean the target object")]
        [Min(1)]
        [SerializeField] private int _cleanUvTargetMaxVertCheckCount;
        protected int CleanUvTargetMaxVertCheckCount => _cleanUvTargetMaxVertCheckCount;

        [Tooltip("The range, in UV coordinates, of the area to clean on the target's dirtiness texture")]
        [Min(0f)]
        [SerializeField] private float _cleanTexRange;
        protected float CleanTexRange => _cleanTexRange;

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

        protected void TryClean(DirtyBoneController dirtyBone, Vector3 point)
        {
            if (CheckCanClean())
                PerformCleanAction(dirtyBone, point);
        }

        protected bool CheckCanClean()
            => Rigidbody.velocity.sqrMagnitude >= MinimumCleanSpeedSqr
            && (!m_LastCleanActionTime.HasValue || Time.time >= m_LastCleanActionTime.Value + CleanActionDelay);

        protected void PerformCleanAction(DirtyBoneController dirtyBone, Vector3 point)
        {
            m_LastCleanActionTime = Time.time;

            Vector3 objectSpaceHitPos = dirtyBone.MeshFilter.transform.InverseTransformPoint(point);
            Mesh mesh = dirtyBone.MeshFilter.sharedMesh;

            // Find the index of the closest vertex to the hit point (in object space)

            int idxStep = mesh.vertices.Length / CleanUvTargetMaxVertCheckCount;

            float minSqrDist = Mathf.Infinity;
            int minVertIdx = 0;

            for (int i = 0; i < mesh.vertices.Length; i += idxStep)
            {
                Vector3 vert = mesh.vertices[i];
                float sqrDist = (vert - objectSpaceHitPos).sqrMagnitude;
                if (sqrDist < minSqrDist)
                {
                    minSqrDist = sqrDist;
                    minVertIdx = i;
                }
            }

            Vector2 uv = mesh.uv[minVertIdx];
            float u = uv.x;
            float v = uv.y;

            dirtyBone.DrawCircle(0f, u, v, CleanTexRange);
        }
    }
}
