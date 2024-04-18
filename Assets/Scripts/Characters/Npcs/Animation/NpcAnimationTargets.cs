using System;
using UnityEngine;
using static BonesVr.Characters.Npcs.Animation.NpcAnimationClip;

namespace BonesVr.Characters.Npcs.Animation
{
    [Serializable]
    public struct NpcAnimationTargets
    {
        public Transform m_Root;
        public Transform m_Head;
        // TODO - right and left hands

        public readonly NpcAnimationClip.Snapshot CreateSnapshot()
            => new() {
                rootLocalPosition = m_Root.localPosition,
                rootLocalRotation = m_Root.localRotation,

                headLocalPosition = m_Head.localPosition,
                headLocalRotation = m_Head.localRotation,
            };

        public readonly void ApplySnapshot(Snapshot snap)
        {
            m_Root.transform.SetLocalPositionAndRotation(snap.rootLocalPosition, snap.rootLocalRotation);
            m_Head.transform.SetLocalPositionAndRotation(snap.headLocalPosition, snap.headLocalRotation);
        }
    }
}
