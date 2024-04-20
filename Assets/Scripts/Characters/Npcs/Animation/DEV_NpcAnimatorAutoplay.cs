using UnityEngine;

namespace BonesVr.Characters.Npcs.Animation
{
    [RequireComponent(typeof(NpcAnimator))]
    public class DEV_NpcAnimatorAutoplay : MonoBehaviour
    {
        protected NpcAnimator Animator => GetComponent<NpcAnimator>();

        private NpcAnimationClip m_PrevClip;

        [SerializeField] private NpcAnimationClip _clip;
        protected NpcAnimationClip Clip => _clip;

        protected virtual void Start()
        {
            m_PrevClip = null;

            if (Clip != null)
                StartSelectedClip();
        }

        protected virtual void Update()
        {
            if (Clip != m_PrevClip)
                StartSelectedClip();
        }

        public void StartSelectedClip()
        {
            Animator.StartClip(Clip);
            m_PrevClip = Clip;
        }
    }
}
