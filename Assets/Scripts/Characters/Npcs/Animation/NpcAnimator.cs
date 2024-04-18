using System;
using UnityEngine;

namespace BonesVr.Characters.Npcs.Animation
{
    public class NpcAnimator : MonoBehaviour
    {
        [SerializeField] private NpcAnimationTargets _animationTargets;
        protected NpcAnimationTargets AnimationTargets => _animationTargets;

        [Tooltip("If set, this clip will start being played on Start")]
        [SerializeField] private NpcAnimationClip _initialClip = null;

        private NpcAnimationClip m_CurrClip = null;
        private int m_CurrKeyframeIdx;
        private float m_CurrClipStartTime;

        public bool IsPlaying => m_CurrClip != null;

        protected virtual void Start()
        {
            if (_initialClip != null)
                StartClip(_initialClip);
            else
                StopClip();
        }

        protected virtual void Update()
        {
            if (IsPlaying)
            {
                float t = Time.time - m_CurrClipStartTime;

                // Move forward through active keyframes if possible and needed
                while (m_CurrKeyframeIdx < m_CurrClip.Keyframes.Length - 1 && t >= m_CurrClip.Keyframes[m_CurrKeyframeIdx + 1].time)
                    m_CurrKeyframeIdx++;

                if (m_CurrKeyframeIdx == m_CurrClip.Keyframes.Length - 1)
                {
                    // On last keyframe avaliable
                    AnimationTargets.ApplyKeyframe(m_CurrClip.Keyframes[m_CurrKeyframeIdx].keyframe);
                }
                else
                {
                    NpcAnimationClip.KeyframePair pairA = m_CurrClip.Keyframes[m_CurrKeyframeIdx];
                    float aTime = pairA.time;
                    NpcAnimationClip.Keyframe a = pairA.keyframe;
                    NpcAnimationClip.Keyframe b = m_CurrClip.Keyframes[m_CurrKeyframeIdx + 1].keyframe;

                    AnimationTargets.ApplyKeyframe(NpcAnimationClip.Keyframe.Interpolate(a, b, t - aTime));
                }
            }
        }

        public void StartClip(NpcAnimationClip clip)
        {
            m_CurrClip = clip;
            m_CurrKeyframeIdx = 0;
            m_CurrClipStartTime = Time.time;
        }

        /// <summary>
        /// If there is a clip playing, stop it playing.
        /// </summary>
        public void StopClip()
        {
            m_CurrClip = null;
        }
    }
}
