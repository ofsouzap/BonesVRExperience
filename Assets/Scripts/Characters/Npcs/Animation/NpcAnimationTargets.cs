using System;
using UnityEngine;

namespace BonesVr.Characters.Npcs.Animation
{
    [Serializable]
    public struct NpcAnimationTargets
    {
        public Transform m_Root;
        public Transform m_Head;
        // TODO - right and left hands

        public readonly NpcAnimationClip.Keyframe CreateKeyframe()
            => new() {
                m_RootLocalPosition = m_Root.localPosition,
                m_RootLocalRotation = m_Root.localRotation,

                m_HeadLocalPosition = m_Head.localPosition,
                m_HeadLocalRotation = m_Head.localRotation,
            };

        public readonly void ApplyKeyframe(NpcAnimationClip.Keyframe kf)
        {
            m_Root.transform.SetLocalPositionAndRotation(kf.m_RootLocalPosition, kf.m_RootLocalRotation);
            m_Head.transform.SetLocalPositionAndRotation(kf.m_HeadLocalPosition, kf.m_HeadLocalRotation);
        }
    }
}
