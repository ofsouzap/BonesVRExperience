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
                m_CurrIdx = 0;
            }

            public T GetValue(float t)
            {
                // If no keyframes defined
                if (Keyframes.Count == 0)
                    return default;

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
                    var kfB = Keyframes[m_CurrIdx + 1];
                    float bTime = kfB.time;
                    var b = kfB.val;

                    float tAB = (t - aTime) / (bTime - aTime);

                    return m_InterpolateFunc(a, b, tAB);
                }
            }
        }

        protected struct TargetTrackAnimators
        {
            private static string InterpolateTextBoxes(string a, string b, float t) => t < 1f ? a : b;

            private static bool InterpolateBools(bool a, bool b, float t)
                => t < .5f ? a : b;

            public TrackAnimator<string> textBox;

            public TrackAnimator<Vector3> rootLocalPosition;
            public TrackAnimator<Quaternion> rootLocalRotation;

            public TrackAnimator<Vector3> headLocalPosition;
            public TrackAnimator<Quaternion> headLocalRotation;

            public TrackAnimator<Vector3> RHLocalPosition;
            public TrackAnimator<Quaternion> RHLocalRotation;
            public TrackAnimator<bool> RHThumbTouched;
            public TrackAnimator<bool> RHIndexTouched;
            public TrackAnimator<bool> RHGripTouched;
            public TrackAnimator<float> RHThumbVal;
            public TrackAnimator<float> RHIndexVal;
            public TrackAnimator<float> RHGripVal;

            public TrackAnimator<Vector3> LHLocalPosition;
            public TrackAnimator<Quaternion> LHLocalRotation;
            public TrackAnimator<bool> LHThumbTouched;
            public TrackAnimator<bool> LHIndexTouched;
            public TrackAnimator<bool> LHGripTouched;
            public TrackAnimator<float> LHThumbVal;
            public TrackAnimator<float> LHIndexVal;
            public TrackAnimator<float> LHGripVal;

            public TargetTrackAnimators(
                ITrack<string> textBoxTrack,

                ITrack<Vector3> rootLocalPositionTrack,
                ITrack<Quaternion> rootLocalRotationTrack,

                ITrack<Vector3> headLocalPositionTrack,
                ITrack<Quaternion> headLocalRotationTrack,

                ITrack<Vector3> RHLocalPositionTrack,
                ITrack<Quaternion> RHLocalRotationTrack,
                ITrack<bool> RHThumbTouchedTrack,
                ITrack<bool> RHIndexTouchedTrack,
                ITrack<bool> RHGripTouchedTrack,
                ITrack<float> RHThumbValTrack,
                ITrack<float> RHIndexValTrack,
                ITrack<float> RHGripValTrack,

                ITrack<Vector3> LHLocalPositionTrack,
                ITrack<Quaternion> LHLocalRotationTrack,
                ITrack<bool> LHThumbTouchedTrack,
                ITrack<bool> LHIndexTouchedTrack,
                ITrack<bool> LHGripTouchedTrack,
                ITrack<float> LHThumbValTrack,
                ITrack<float> LHIndexValTrack,
                ITrack<float> LHGripValTrack
                )
            {
                textBox = new(textBoxTrack, InterpolateTextBoxes);

                rootLocalPosition = new(rootLocalPositionTrack, Vector3.Lerp);
                rootLocalRotation = new(rootLocalRotationTrack, Quaternion.Lerp);

                headLocalPosition = new(headLocalPositionTrack, Vector3.Lerp);
                headLocalRotation = new(headLocalRotationTrack, Quaternion.Lerp);

                RHLocalPosition = new(RHLocalPositionTrack, Vector3.Lerp);
                RHLocalRotation = new(RHLocalRotationTrack, Quaternion.Lerp);
                RHThumbTouched = new(RHThumbTouchedTrack, InterpolateBools);
                RHIndexTouched = new(RHIndexTouchedTrack, InterpolateBools);
                RHGripTouched = new(RHGripTouchedTrack, InterpolateBools);
                RHThumbVal = new(RHThumbValTrack, Mathf.Lerp);
                RHIndexVal = new(RHIndexValTrack, Mathf.Lerp);
                RHGripVal = new(RHGripValTrack, Mathf.Lerp);

                LHLocalPosition = new(LHLocalPositionTrack, Vector3.Lerp);
                LHLocalRotation = new(LHLocalRotationTrack, Quaternion.Lerp);
                LHThumbTouched = new(LHThumbTouchedTrack, InterpolateBools);
                LHIndexTouched = new(LHIndexTouchedTrack, InterpolateBools);
                LHGripTouched = new(LHGripTouchedTrack, InterpolateBools);
                LHThumbVal = new(LHThumbValTrack, Mathf.Lerp);
                LHIndexVal = new(LHIndexValTrack, Mathf.Lerp);
                LHGripVal = new(LHGripValTrack, Mathf.Lerp);
            }

            public Snapshot GetSnapshot(float t)
                => new()
                {
                    textBox = textBox.GetValue(t),

                    rootLocalPosition = rootLocalPosition.GetValue(t),
                    rootLocalRotation = rootLocalRotation.GetValue(t),

                    headLocalPosition = headLocalPosition.GetValue(t),
                    headLocalRotation = headLocalRotation.GetValue(t),

                    RHLocalPosition = RHLocalPosition.GetValue(t),
                    RHLocalRotation = RHLocalRotation.GetValue(t),
                    RHThumbTouched = RHThumbTouched.GetValue(t),
                    RHIndexTouched = RHIndexTouched.GetValue(t),
                    RHGripTouched = RHGripTouched.GetValue(t),
                    RHThumbVal = RHThumbVal.GetValue(t),
                    RHIndexVal = RHIndexVal.GetValue(t),
                    RHGripVal = RHGripVal.GetValue(t),

                    LHLocalPosition = LHLocalPosition.GetValue(t),
                    LHLocalRotation = LHLocalRotation.GetValue(t),
                    LHThumbTouched = LHThumbTouched.GetValue(t),
                    LHIndexTouched = LHIndexTouched.GetValue(t),
                    LHGripTouched = LHGripTouched.GetValue(t),
                    LHThumbVal = LHThumbVal.GetValue(t),
                    LHIndexVal = LHIndexVal.GetValue(t),
                    LHGripVal = LHGripVal.GetValue(t),
                };
        }

        public readonly struct PlayClipOptions
        {
            public readonly Action onClipFinished;

            public PlayClipOptions(Action onClipFinished = null)
            {
                this.onClipFinished = onClipFinished;
            }
        }

        [SerializeField] private NpcAnimationAnimatingTargets _animationTargets;
        protected NpcAnimationAnimatingTargets AnimationTargets => _animationTargets;

        [Tooltip("If set, this clip will start being played on Start")]
        [SerializeField] private NpcAnimationClip _initialClip = null;

        private NpcAnimationClip m_CurrClip = null;
        private TargetTrackAnimators? m_TargetTrackAnimators = null;
        private float m_CurrClipStartTime;
        private PlayClipOptions? m_CurrClipOptions = null;

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
                AnimationTargets.ApplySnapshot(m_TargetTrackAnimators.Value.GetSnapshot(t));
                if (t > m_CurrClip.m_Duration)
                    EndCurrClip();
            }
        }

        private void EndCurrClip()
        {
            m_CurrClipOptions?.onClipFinished?.Invoke();

            m_CurrClip = null;
            m_TargetTrackAnimators = null;
            m_CurrClipStartTime = 0f;
            m_CurrClipOptions = null;
        }

        public void StartClip(NpcAnimationClip clip, PlayClipOptions options)
        {
            m_CurrClip = clip;
            m_TargetTrackAnimators = new(
                clip.m_TextBox,

                clip.m_RootLocalPosition,
                clip.m_RootLocalRotation,

                clip.m_HeadLocalPosition,
                clip.m_HeadLocalRotation,

                clip.m_RHLocalPosition,
                clip.m_RHLocalRotation,
                clip.m_RHThumbTouched,
                clip.m_RHIndexTouched,
                clip.m_RHGripTouched,
                clip.m_RHThumbVal,
                clip.m_RHIndexVal,
                clip.m_RHGripVal,

                clip.m_LHLocalPosition,
                clip.m_LHLocalRotation,
                clip.m_LHThumbTouched,
                clip.m_LHIndexTouched,
                clip.m_LHGripTouched,
                clip.m_LHThumbVal,
                clip.m_LHIndexVal,
                clip.m_LHGripVal
            );
            m_CurrClipStartTime = Time.time;
            m_CurrClipOptions = options;
        }

        public void StartClip(NpcAnimationClip clip)
            => StartClip(clip, new());

        /// <summary>
        /// If there is a clip playing, stop it playing.
        /// </summary>
        public void StopClip()
        {
            if (IsPlaying)
                EndCurrClip();
        }
    }
}
