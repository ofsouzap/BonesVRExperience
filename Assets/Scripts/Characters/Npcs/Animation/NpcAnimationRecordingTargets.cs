using BonesVr.Characters.Hands;
using System;
using UnityEngine;
using static BonesVr.Characters.Npcs.Animation.NpcAnimationClip;

namespace BonesVr.Characters.Npcs.Animation
{
    [Serializable]
    public struct NpcAnimationRecordingTargets
    {
        public Transform m_Root;
        public Transform m_Head;
        public Transform m_RHTransform;
        public Hand m_RH;
        public Transform m_LHTransform;
        public Hand m_LH;

        public readonly Snapshot CreateSnapshot()
            => new() {
                rootLocalPosition = m_Root.localPosition,
                rootLocalRotation = m_Root.localRotation,

                headLocalPosition = m_Head.localPosition,
                headLocalRotation = m_Head.localRotation,

                RHLocalPosition = m_RHTransform.localPosition,
                RHLocalRotation = m_RHTransform.localRotation,
                RHThumbTouched = m_RH.ThumbTouched,
                RHIndexTouched = m_RH.IndexTouched,
                RHGripTouched = m_RH.GripTouched,
                RHThumbVal = m_RH.ThumbVal,
                RHIndexVal = m_RH.IndexVal,
                RHGripVal = m_RH.GripVal,

                LHLocalPosition = m_LHTransform.localPosition,
                LHLocalRotation = m_LHTransform.localRotation,
                LHThumbTouched = m_LH.ThumbTouched,
                LHIndexTouched = m_LH.IndexTouched,
                LHGripTouched = m_LH.GripTouched,
                LHThumbVal = m_LH.ThumbVal,
                LHIndexVal = m_LH.IndexVal,
                LHGripVal = m_LH.GripVal,
            };

        public readonly void ApplySnapshot(Snapshot snap)
        {
            m_Root.transform.SetLocalPositionAndRotation(snap.rootLocalPosition, snap.rootLocalRotation);

            m_Head.transform.SetLocalPositionAndRotation(snap.headLocalPosition, snap.headLocalRotation);

            m_RHTransform.SetLocalPositionAndRotation(snap.RHLocalPosition, snap.RHLocalRotation);
        }
    }
}
