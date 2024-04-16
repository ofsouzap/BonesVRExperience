using UnityEngine;

namespace BonesVr.Minigames.Cleaning
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DirtParticleSprayController : MonoBehaviour
    {
        public struct Config
        {
        }

        protected ParticleSystem ParticleSystem => GetComponent<ParticleSystem>();

        [SerializeField] private bool _destroyOnFinished = true;
        protected bool DestroyOnFinished => _destroyOnFinished;

        private float m_DestroyTime;

        protected virtual void Start()
        {
            m_DestroyTime = Time.time + ParticleSystem.main.startLifetime.constantMax;
        }

        protected virtual void Update()
        {
            if (DestroyOnFinished && Time.time > m_DestroyTime)
                Destroy(gameObject);
        }

        public void SetConfig(Config config)
        {
        }
    }
}
