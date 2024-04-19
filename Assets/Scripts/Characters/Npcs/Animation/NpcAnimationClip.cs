using System;
using System.Collections.Generic;
using UnityEngine;

namespace BonesVr.Characters.Npcs.Animation
{
    public class NpcAnimationClip : ScriptableObject
    {
        public struct Snapshot
        {
            public string textBox;

            public Vector3 rootLocalPosition;
            public Quaternion rootLocalRotation;

            public Vector3 headLocalPosition;
            public Quaternion headLocalRotation;

            public Vector3 RHLocalPosition;
            public Quaternion RHLocalRotation;
            public bool RHThumbTouched;
            public bool RHIndexTouched;
            public bool RHGripTouched;
            public float RHThumbVal;
            public float RHIndexVal;
            public float RHGripVal;

            public Vector3 LHLocalPosition;
            public Quaternion LHLocalRotation;
            public bool LHThumbTouched;
            public bool LHIndexTouched;
            public bool LHGripTouched;
            public float LHThumbVal;
            public float LHIndexVal;
            public float LHGripVal;
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
        public class TextBoxTrack : ITrack<string>
        {
            public List<Keyframe<string>> m_Keyframes;

            public TextBoxTrack()
            {
                m_Keyframes = new();
            }

            public List<Keyframe<string>> GetKeyframes() => m_Keyframes;

            public void GiveNextValue(float t, string val)
            {
                m_Keyframes.Add(new(t, val));
            }
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

        [Serializable]
        public class AnimFloatTrack : ITrack<float>
        {
            private const float c_Eps = 1e-1f;

            public List<Keyframe<float>> m_Keyframes;

            public AnimFloatTrack()
            {
                m_Keyframes = new();
            }

            public List<Keyframe<float>> GetKeyframes() => m_Keyframes;

            public void GiveNextValue(float t, float val)
            {
                if (GetKeyframes().Count == 0
                    || Mathf.Abs(val - GetKeyframes()[^1].val) > c_Eps)
                    m_Keyframes.Add(new(t, val));
            }
        }

        [Serializable]
        public class BoolTrack : ITrack<bool>
        {
            public List<Keyframe<bool>> m_Keyframes;

            public BoolTrack()
            {
                m_Keyframes = new();
            }

            public List<Keyframe<bool>> GetKeyframes() => m_Keyframes;

            public void GiveNextValue(float t, bool val)
            {
                if (GetKeyframes().Count == 0
                    || val != GetKeyframes()[^1].val)
                    m_Keyframes.Add(new(t, val));
            }
        }

        public TextBoxTrack m_TextBox = new();

        public Vector3Track m_RootLocalPosition = new();
        public QuaternionTrack m_RootLocalRotation = new();

        public Vector3Track m_HeadLocalPosition = new();
        public QuaternionTrack m_HeadLocalRotation = new();

        public Vector3Track m_RHLocalPosition = new();
        public QuaternionTrack m_RHLocalRotation = new();
        public BoolTrack m_RHThumbTouched = new();
        public BoolTrack m_RHIndexTouched = new();
        public BoolTrack m_RHGripTouched = new();
        public AnimFloatTrack m_RHThumbVal = new();
        public AnimFloatTrack m_RHIndexVal = new();
        public AnimFloatTrack m_RHGripVal = new();

        public Vector3Track m_LHLocalPosition = new();
        public QuaternionTrack m_LHLocalRotation = new();
        public BoolTrack m_LHThumbTouched = new();
        public BoolTrack m_LHIndexTouched = new();
        public BoolTrack m_LHGripTouched = new();
        public AnimFloatTrack m_LHThumbVal = new();
        public AnimFloatTrack m_LHIndexVal = new();
        public AnimFloatTrack m_LHGripVal = new();

        /// <summary>
        /// Give new values for a certain time.
        /// Note that the value on the text box track is ignored as adding text box track keyframes should be handled elsewhere.
        /// </summary>
        public void GiveNextSnapshot(float t, Snapshot snap)
        {
            m_RootLocalPosition.GiveNextValue(t, snap.rootLocalPosition);
            m_RootLocalRotation.GiveNextValue(t, snap.rootLocalRotation);

            m_HeadLocalPosition.GiveNextValue(t, snap.headLocalPosition);
            m_HeadLocalRotation.GiveNextValue(t, snap.headLocalRotation);

            m_RHLocalPosition.GiveNextValue(t, snap.RHLocalPosition);
            m_RHLocalRotation.GiveNextValue(t, snap.RHLocalRotation);
            m_RHThumbTouched.GiveNextValue(t, snap.RHThumbTouched);
            m_RHIndexTouched.GiveNextValue(t, snap.RHIndexTouched);
            m_RHGripTouched.GiveNextValue(t, snap.RHGripTouched);
            m_RHThumbVal.GiveNextValue(t, snap.RHThumbVal);
            m_RHIndexVal.GiveNextValue(t, snap.RHIndexVal);
            m_RHGripVal.GiveNextValue(t, snap.RHGripVal);

            m_LHLocalPosition.GiveNextValue(t, snap.LHLocalPosition);
            m_LHLocalRotation.GiveNextValue(t, snap.LHLocalRotation);
            m_LHThumbTouched.GiveNextValue(t, snap.LHThumbTouched);
            m_LHIndexTouched.GiveNextValue(t, snap.LHIndexTouched);
            m_LHGripTouched.GiveNextValue(t, snap.LHGripTouched);
            m_LHThumbVal.GiveNextValue(t, snap.LHThumbVal);
            m_LHIndexVal.GiveNextValue(t, snap.LHIndexVal);
            m_LHGripVal.GiveNextValue(t, snap.LHGripVal);
        }

        /// <summary>
        /// Add a keyframe for a text box value
        /// </summary>
        public void AddTextBoxKeyframe(float t, string text = null)
        {
            m_TextBox.GiveNextValue(t, text);
        }
    }
}
