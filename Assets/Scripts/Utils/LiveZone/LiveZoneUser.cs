using UnityEngine;

namespace BonesVr.Utils.LiveZone
{
    public class LiveZoneUser : MonoBehaviour
    {
        [Tooltip("The live zone volume that this game object should stay within to avoid respawning")]
        [SerializeField] private LiveZoneVolume _associatedVolume = null;
        protected LiveZoneVolume AssociatedVolume => _associatedVolume;

        [Tooltip("How long the game object can be outside of its live zone without having to respawn")]
        [SerializeField] private float _respawnGracePeriod = 3f;
        protected float RespawnGracePeriod => _respawnGracePeriod;

        private Vector3 m_SpawnPos;
        private float? m_RespawnTimerStart;

        protected virtual void Awake()
        {
            if (AssociatedVolume == null)
                Debug.LogWarning("No associated live zone volume has been set");
        }

        protected virtual void Start()
        {
            m_SpawnPos = transform.position;
            m_RespawnTimerStart = null;
        }

        protected virtual void Update()
        {
            if (m_RespawnTimerStart != null)
                if (Time.time > m_RespawnTimerStart + RespawnGracePeriod)
                    Respawn();
        }

        protected virtual void OnValidate()
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            if (colliders.Length == 0)
                Debug.LogError("Live zone user must have at least one collider in its or its childrens' game objects");
        }

        protected void Respawn()
        {
            m_RespawnTimerStart = null; // This should actually be done automatically as this counts as re-entering the trigger zone but this is just in case
            print($"Cleared timer for {gameObject.name}");

            transform.position = m_SpawnPos;

            if (TryGetComponent<Rigidbody>(out var rb) && !rb.isKinematic)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        public void ExitedLiveZoneVolume(LiveZoneVolume liveZoneVolume)
        {
            if (liveZoneVolume == AssociatedVolume)
                StartRespawnTimer();
        }

        public void EnteredLiveZoneVolume(LiveZoneVolume liveZoneVolume)
        {
            if (liveZoneVolume == AssociatedVolume)
                StopRespawnTimer();
        }

        protected void StartRespawnTimer()
        {
            m_RespawnTimerStart = Time.time;
            print($"Started timer for {gameObject.name}");
        }

        protected void StopRespawnTimer()
        {
            m_RespawnTimerStart = null;
            print($"Stopped timer for {gameObject.name}");
        }
    }
}
