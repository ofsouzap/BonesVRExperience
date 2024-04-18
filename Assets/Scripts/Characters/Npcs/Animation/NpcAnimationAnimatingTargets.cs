using BonesVr.Characters.Hands;
using System;
using UnityEngine;
using static BonesVr.Characters.Npcs.Animation.NpcAnimationClip;

namespace BonesVr.Characters.Npcs.Animation
{
    [Serializable]
    public struct NpcAnimationAnimatingTargets
    {
        public Transform m_Root;
        public Transform m_Head;
        public Transform m_RHTransform;
        public NpcAnimatedHandController m_RH;
        public Transform m_LHTransform;
        public NpcAnimatedHandController m_LH;

        public readonly void ApplySnapshot(Snapshot snap)
        {
            m_Root.transform.SetLocalPositionAndRotation(snap.rootLocalPosition, snap.rootLocalRotation);

            m_Head.transform.SetLocalPositionAndRotation(snap.headLocalPosition, snap.headLocalRotation);

            m_RHTransform.SetLocalPositionAndRotation(snap.RHLocalPosition, snap.RHLocalRotation);
            m_RH.ThumbTouched = snap.RHThumbTouched;
            m_RH.IndexTouched = snap.RHIndexTouched;
            m_RH.GripTouched = snap.RHGripTouched;
            m_RH.ThumbVal = snap.RHThumbVal;
            m_RH.IndexVal = snap.RHIndexVal;
            m_RH.GripVal = snap.RHGripVal;

            m_LHTransform.SetLocalPositionAndRotation(snap.LHLocalPosition, snap.LHLocalRotation);
            m_LH.ThumbTouched = snap.LHThumbTouched;
            m_LH.IndexTouched = snap.LHIndexTouched;
            m_LH.GripTouched = snap.LHGripTouched;
            m_LH.ThumbVal = snap.LHThumbVal;
            m_LH.IndexVal = snap.LHIndexVal;
            m_LH.GripVal = snap.LHGripVal;
        }
    }
}
