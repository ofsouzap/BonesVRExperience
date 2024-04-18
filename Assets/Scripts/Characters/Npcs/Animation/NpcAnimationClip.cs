using System;
using System.Collections.Generic;
using UnityEngine;

namespace BonesVr.Characters.Npcs.Animation
{
    public class NpcAnimationClip : ScriptableObject
    {
        public struct Snapshot
        {
            public Vector3 rootLocalPosition;
            public Quaternion rootLocalRotation;
            public Vector3 headLocalPosition;
            public Quaternion headLocalRotation;
        }

        [Serializable]
        public struct Keyframe<T>
        {
            public float time;
            public T val;

            public Keyframe(float time, T val)
            {
                this.time = time;
                this.val = val;
            }
        }

        public interface ITrack<T>
        {
            public List<Keyframe<T>> GetKeyframes();

            /// <summary>
            /// Give the track a value for the current end of the track.
            /// The track implementation may choose to store this value or discard it if it is too close to the current end value.
            /// </summary>
            public void GiveNextValue(float t, T val);
        }

        [Serializable]
        public class Vector3Track : ITrack<Vector3>
        {
            private const float c_Eps = 1e-3f;

            public List<Keyframe<Vector3>> m_Keyframes;

            public Vector3Track()
            {
                m_Keyframes = new();
            }

            public List<Keyframe<Vector3>> GetKeyframes() => m_Keyframes;

            public void GiveNextValue(float t, Vector3 val)
            {
                if (GetKeyframes().Count == 0
                    || (val - GetKeyframes()[^1].val).sqrMagnitude > c_Eps)
                    m_Keyframes.Add(new(t, val));
            }
        }

        [Serializable]
        public class QuaternionTrack : ITrack<Quaternion>
        {
            private const float c_Eps = Mathf.PI / 30f;

            public List<Keyframe<Quaternion>> m_Keyframes;

            public QuaternionTrack()
            {
                m_Keyframes = new();
            }

            public List<Keyframe<Quaternion>> GetKeyframes() => m_Keyframes;

            public void GiveNextValue(float t, Quaternion val)
            {
                if (GetKeyframes().Count == 0
                    || Mathf.Abs(Quaternion.Angle(val, GetKeyframes()[^1].val)) > c_Eps) // Compare epsilon value to the angle between the two rotation quaternions
                    m_Keyframes.Add(new(t, val));
            }
        }

        public Vector3Track m_RootLocalPosition = new();
        public QuaternionTrack m_RootLocalRotation = new();

        public Vector3Track m_HeadLocalPosition = new();
        public QuaternionTrack m_HeadLocalRotation = new();

        public void GiveNextSnapshot(float t, Snapshot snap)
        {
            m_RootLocalPosition.GiveNextValue(t, snap.rootLocalPosition);
            m_RootLocalRotation.GiveNextValue(t, snap.rootLocalRotation);
            m_HeadLocalPosition.GiveNextValue(t, snap.headLocalPosition);
            m_HeadLocalRotation.GiveNextValue(t, snap.headLocalRotation);
        }
    }
}
