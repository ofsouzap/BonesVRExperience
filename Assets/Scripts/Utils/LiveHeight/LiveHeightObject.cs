using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BonesVr.Utils.LiveHeight
{
    public class LiveHeightObject : MonoBehaviour
    {
        private static readonly Color c_LiveHeightGizmoColor = new(0f, 0f, 1f);
        private const float c_GizmoWidth = 1f;

        [Tooltip("The minimum Y position that the transform should be at before looking to respawn")]
        [SerializeField] private float _liveMinHeight = 0f;
        protected float LiveMinHeight => _liveMinHeight;

        [Tooltip("Whether the object's rotation should also be reset when it is respawned")]
        [SerializeField] private bool _resetRotation = true;
        protected bool ResetRotation => _resetRotation;

        [Tooltip("How long the game object can be below its live height without having to respawn")]
        [SerializeField] private float _respawnGracePeriod = 3f;
        protected float RespawnGracePeriod => _respawnGracePeriod;

        private Vector3 m_SpawnPos;
        private Quaternion m_SpawnRot;
        private float? m_RespawnTimerStart;

        protected virtual void Start()
        {
            m_SpawnPos = transform.position;
            m_SpawnRot = transform.rotation;
            m_RespawnTimerStart = null;
        }

        protected virtual void Update()
        {
            if (m_RespawnTimerStart != null)
            {
                if (Time.time > m_RespawnTimerStart + RespawnGracePeriod)
                    Respawn();
            }
            else if (CheckReadyToRespawn())
                StartRespawnTimer();
            else
                StopRespawnTimer();
        }

        protected virtual void OnValidate()
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            if (colliders.Length == 0)
                Debug.LogError("Live zone user must have at least one collider in its or its childrens' game objects");
        }

        protected virtual void OnDrawGizmosSelected()
        {
            // Draw a plane at the object's live height boundary
            Gizmos.color = c_LiveHeightGizmoColor;
            Gizmos.DrawCube(
                new(transform.position.x, LiveMinHeight, transform.position.z),
                new(c_GizmoWidth, 0f, c_GizmoWidth)
            );
        }

        protected void Respawn()
        {
            m_RespawnTimerStart = null; // This should actually be done automatically as this counts as re-entering the live height region but this is just in case

            transform.position = m_SpawnPos;

            if (ResetRotation)
                transform.rotation = m_SpawnRot;

            if (TryGetComponent<Rigidbody>(out var rb) && !rb.isKinematic)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        protected bool IsAHeldXrInteractable()
        {
            XRBaseInteractable interactable = GetComponentInChildren<XRBaseInteractable>();
            return interactable != null && interactable.isSelected;
        }

        /// <summary>
        /// Check if the game object is in a state where it should start its respawn timer, including checking its height.
        /// </summary>
        protected bool CheckReadyToRespawn()
            => transform.position.y < LiveMinHeight
            && !IsAHeldXrInteractable();

        protected void StartRespawnTimer()
        {
            m_RespawnTimerStart = Time.time;
        }

        protected void StopRespawnTimer()
        {
            m_RespawnTimerStart = null;
        }
    }
}
