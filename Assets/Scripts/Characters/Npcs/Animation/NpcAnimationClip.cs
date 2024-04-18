using System;
using UnityEngine;

namespace BonesVr.Characters.Npcs.Animation
{
    public class NpcAnimationClip : ScriptableObject
    {
        [Serializable]
        public struct Keyframe
        {
            public Vector3 m_RootLocalPosition;
            public Quaternion m_RootLocalRotation;

            public Vector3 m_HeadLocalPosition;
            public Quaternion m_HeadLocalRotation;

            // TODO - right and left hands

            public static Keyframe Interpolate(Keyframe a, Keyframe b, float t)
                => new() {
                    m_RootLocalPosition = Vector3.Lerp(a.m_RootLocalPosition, b.m_RootLocalPosition, t),
                    m_RootLocalRotation = Quaternion.Lerp(a.m_RootLocalRotation, b.m_RootLocalRotation, t),

                    m_HeadLocalPosition = Vector3.Lerp(a.m_HeadLocalPosition, b.m_HeadLocalPosition, t),
                    m_HeadLocalRotation = Quaternion.Lerp(a.m_HeadLocalRotation, b.m_HeadLocalRotation, t),
                };
        }

        [Serializable]
        public struct KeyframePair
        {
            public float time;
            public Keyframe keyframe;

            public KeyframePair(float time, Keyframe keyframe)
            {
                this.time = time;
                this.keyframe = keyframe;
            }
        }

        /// <summary>
        /// The keyframes for the clip.
        /// These must be in increasing order.
        /// The float value for a keyframe is the absolute time of it with time 0 representing when the clip starts. The time value of the first keyframe might be ignored.
        /// </summary>
        [SerializeField] private KeyframePair[] m_Keyframes = new KeyframePair[0];

        public KeyframePair[] Keyframes => m_Keyframes;

        /// <summary>
        /// Sets the keyframes to a copy of the parameter
        /// </summary>
        private void SetKeyframes(KeyframePair[] keyframes)
        {
            m_Keyframes = new KeyframePair[keyframes.Length];
            Array.Copy(keyframes, m_Keyframes, Keyframes.Length);
        }

        public static NpcAnimationClip CreateFromKeyframes(KeyframePair[] keyframes)
        {
            NpcAnimationClip obj = CreateInstance<NpcAnimationClip>();
            obj.SetKeyframes(keyframes);
            return obj;
        }
    }
}
