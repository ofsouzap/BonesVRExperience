using System;
using System.Collections.Generic;
using UnityEngine;
using static BonesVr.Characters.Npcs.Animation.NpcAnimationClip;

namespace BonesVr.Characters.Npcs.Animation
{
    public class NpcAnimator : MonoBehaviour
    {
        protected class TrackAnimator<T>
        {
            private readonly ITrack<T> m_Track;
            private List<Keyframe<T>> Keyframes => m_Track.GetKeyframes();

            private readonly Func<T, T, float, T> m_InterpolateFunc;

            private int m_CurrIdx;

            public TrackAnimator(ITrack<T> track, Func<T, T, float, T> interpolateFunc)
            {
                m_Track = track;
                m_InterpolateFunc = interpolateFunc;
            }

            public T GetValue(float t)
            {
                // Move forward through active keyframes if possible and needed
                while (m_CurrIdx < Keyframes.Count - 1 && t >= Keyframes[m_CurrIdx + 1].time)
                    m_CurrIdx++;

                if (m_CurrIdx == Keyframes.Count - 1)
                {
                    // On last keyframe avaliable
                    return Keyframes[m_CurrIdx].val;
                }
                else
                {
                    // Interpolate between two neighbouring keyframes
                    var kfA = Keyframes[m_CurrIdx];
                    float aTime = kfA.time;
                    var a = kfA.val;
                    var b = Keyframes[m_CurrIdx + 1].val;

                    return m_InterpolateFunc(a, b, t - aTime);
                }
            }
        }

        protected struct TargetTrackAnimators
        {
            public TrackAnimator<Vector3> rootLocalPosition;
            public TrackAnimator<Quaternion> rootLocalRotation;

            public TrackAnimator<Vector3> headLocalPosition;
            public TrackAnimator<Quaternion> headLocalRotation;

            public TargetTrackAnimators(
                ITrack<Vector3> rootLocalPositionTrack,
                ITrack<Quaternion> rootLocalRotationTrack,
                ITrack<Vector3> headLocalPositionTrack,
                ITrack<Quaternion> headLocalRotationTrack
                )
            {
                rootLocalPosition = new(rootLocalPositionTrack, Vector3.Lerp);
                rootLocalRotation = new(rootLocalRotationTrack, Quaternion.Lerp);

                headLocalPosition = new(headLocalPositionTrack, Vector3.Lerp);
                headLocalRotation = new(headLocalRotationTrack, Quaternion.Lerp);
            }

            public Snapshot GetSnapshot(float t)
                => new()
                {
                    rootLocalPosition = rootLocalPosition.GetValue(t),
                    rootLocalRotation = rootLocalRotation.GetValue(t),

                    headLocalPosition = headLocalPosition.GetValue(t),
                    headLocalRotation = headLocalRotation.GetValue(t),
                };
        }

        [SerializeField] private NpcAnimationTargets _animationTargets;
        protected NpcAnimationTargets AnimationTargets => _animationTargets;

        [Tooltip("If set, this clip will start being played on Start")]
        [SerializeField] private NpcAnimationClip _initialClip = null;

        private TargetTrackAnimators? m_TargetTrackAnimators = null;
        private float m_CurrClipStartTime;

        public bool IsPlaying => m_TargetTrackAnimators.HasValue;

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
                AnimationTargets.ApplySnapshot(m_TargetTrackAnimators.Value.GetSnapshot(t));
            }
        }

        public void StartClip(NpcAnimationClip clip)
        {
            m_TargetTrackAnimators = new(
                clip.m_RootLocalPosition,
                clip.m_RootLocalRotation,

                clip.m_HeadLocalPosition,
                clip.m_HeadLocalRotation
            );
            m_CurrClipStartTime = Time.time;
        }

        /// <summary>
        /// If there is a clip playing, stop it playing.
        /// </summary>
        public void StopClip()
        {
            m_TargetTrackAnimators = null;
        }
    }
}
