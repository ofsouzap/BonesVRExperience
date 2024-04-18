using System;
using UnityEngine;

namespace BonesVr.Characters.Npcs.Animation
{
    public class NpcAnimator : MonoBehaviour
    {
        [SerializeField] private NpcAnimationTargets _animationTargets;
        protected NpcAnimationTargets AnimationTargets => _animationTargets;

        [SerializeField] private bool _playOnStart = false;
        protected bool PlayOnStart => _playOnStart;

        [Tooltip("If set, this clip will start being played on Start")]
        [SerializeField] private NpcAnimationClip _initialClip = null;

        private NpcAnimationClip m_CurrClip = null;
        private int m_CurrKeyframeIdx;
        private float m_CurrClipStartTime;

        public bool IsPlaying => m_CurrClip != null;

        protected virtual void Start()
        {
            if (_initialClip == null)
                StartClip(m_CurrClip);
            else
                StopClip();
        }

        protected virtual void Update()
        {
            if (IsPlaying)
            {
                float t = Time.time - m_CurrClipStartTime;

                if (m_CurrKeyframeIdx == m_CurrClip.Keyframes.Length - 1)
                {
                    // On last keyframe avaliable
                    AnimationTargets.ApplyKeyframe(m_CurrClip.Keyframes[m_CurrKeyframeIdx].Item2);
                }
                else
                {
                    // Move forward through active keyframes if needed
                    while (t >= m_CurrClip.Keyframes[m_CurrKeyframeIdx + 1].Item1)
                        m_CurrKeyframeIdx++;

                    Tuple<float, NpcAnimationClip.Keyframe> pairA = m_CurrClip.Keyframes[m_CurrKeyframeIdx];
                    float aTime = pairA.Item1;
                    NpcAnimationClip.Keyframe a = pairA.Item2;
                    NpcAnimationClip.Keyframe b = m_CurrClip.Keyframes[m_CurrKeyframeIdx + 1].Item2;

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
