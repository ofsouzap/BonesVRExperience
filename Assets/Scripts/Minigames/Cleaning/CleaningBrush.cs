using UnityEngine;

namespace BonesVr.Minigames.Cleaning
{
    [RequireComponent(typeof(Rigidbody))]
    public class CleaningBrush : MonoBehaviour
    {
        public Rigidbody Rigidbody => GetComponent<Rigidbody>();

        [Header("Cleaning Target")]

        [Tooltip("The transforms for the game object used as the origin points for the cleaning action. The forward direction and position of these transforms help find which parts of a mesh to clean")]
        [SerializeField] private Transform[] _cleaningOrigins = new Transform[0];
        protected Transform[] CleaningOrigins => _cleaningOrigins;

        [Min(0f)]
        [SerializeField] private float _cleanRange;
        protected float CleanRange => _cleanRange;

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

        protected virtual void Awake()
        {
            if (CleaningOrigins == null)
                Debug.LogError("No cleaning origin set");
        }

        protected virtual void Start()
        {
            m_LastCleanActionTime = null;
        }

        protected virtual void Update()
        {
            if (CheckCanClean())
                PerformCleanAction();
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            if (CleaningOrigins != null)
                foreach (Transform cleaningOrigin in CleaningOrigins)
                    Gizmos.DrawLine(cleaningOrigin.position, cleaningOrigin.position + (cleaningOrigin.forward * CleanRange));
        }

        protected bool CheckCanClean()
            => Rigidbody.velocity.sqrMagnitude >= MinimumCleanSpeedSqr
            && (!m_LastCleanActionTime.HasValue || Time.time >= m_LastCleanActionTime.Value + CleanActionDelay);

        protected void PerformCleanAction()
        {
            m_LastCleanActionTime = Time.time;

            foreach (Transform cleaningOrigin in CleaningOrigins)
                if (Physics.Raycast(cleaningOrigin.position, cleaningOrigin.forward, out var hit, CleanRange))
                {
                    var dirtController = hit.transform.GetComponentInParent<DirtyBoneController>();

                    if (dirtController != null)
                    {
                        Vector3 hitPos = hit.point;
                        Vector3 objectSpaceHitPos = dirtController.MeshFilter.transform.InverseTransformPoint(hitPos);
                        Mesh mesh = dirtController.MeshFilter.sharedMesh;

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

                        dirtController.DrawCircle(0f, u, v, CleanTexRange);
                    }
                }
        }
    }
}
