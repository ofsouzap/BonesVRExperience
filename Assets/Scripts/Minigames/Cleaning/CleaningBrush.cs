using UnityEngine;

namespace BonesVr.Minigames.Cleaning
{
    [RequireComponent(typeof(Rigidbody))]
    public class CleaningBrush : MonoBehaviour
    {
        public Rigidbody Rigidbody => GetComponent<Rigidbody>();

        [Header("Cleaning Target")]

        [Tooltip("The transform for the game object used as the origin point for the cleaning action. The forward direction and position of this transform help find which parts of a mesh to clean")]
        [SerializeField] private Transform _cleaningOrigin;
        protected Transform CleaningOrigin => _cleaningOrigin;

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

        [Tooltip("The range, in UV coordinates, of the area to clean on the target's dirtiness texture")]
        [Min(0f)]
        [SerializeField] private float _cleanTexRange;
        protected float CleanTexRange => _cleanTexRange;

        private float? m_LastCleanActionTime;

        private void Awake()
        {
            if (CleaningOrigin == null)
                Debug.LogError("No cleaning origin set");
        }

        protected virtual void Start()
        {
            m_LastCleanActionTime = null;
        }

        private void Update()
        {
            if (CheckCanClean())
                PerformCleanAction();
        }

        protected bool CheckCanClean()
            => Rigidbody.velocity.sqrMagnitude >= MinimumCleanSpeedSqr
            && (!m_LastCleanActionTime.HasValue || Time.time >= m_LastCleanActionTime.Value + CleanActionDelay);

        protected void PerformCleanAction()
        {
            m_LastCleanActionTime = Time.time;

            if (Physics.Raycast(CleaningOrigin.position, CleaningOrigin.forward, out var hit, CleanRange))
            {

                print($"hit {hit.transform.name}");
                var dirtController = hit.transform.GetComponentInParent<DirtyBoneShaderController>();

                if (dirtController != null)
                {
                    print("has dirt controller");
                    Vector3 hitPos = hit.point;
                    Vector3 objectSpaceHitPos = dirtController.MeshFilter.transform.InverseTransformPoint(hitPos);
                    Mesh mesh = dirtController.MeshFilter.sharedMesh;

                    // Find the index of the closest vertex to the hit point (in object space)
                    float minSqrDist = Mathf.Infinity;
                    int minVertIdx = 0;
                    for (int i = 0; i < mesh.vertices.Length; i++)
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
                    print($"({u}, {v})");

                    dirtController.DrawCircle(0f, u, v, CleanTexRange);
                }
            }
        }
    }
}
