using UnityEngine;
using static BonesVr.Characters.Npcs.Animation.NpcAnimationClip;

namespace BonesVr.Characters.Npcs.Animation
{
    public class NpcAnimationClipBuilder
    {
        protected readonly NpcAnimationClip m_AnimationClip;
        protected float m_LatestKeyframe;

        public NpcAnimationClipBuilder()
        {
            m_AnimationClip = ScriptableObject.CreateInstance<NpcAnimationClip>();
        }

        public void AddKeyframe(float t, Snapshot snap)
        {
            m_AnimationClip.GiveNextSnapshot(t, snap);

            if (t > m_LatestKeyframe)
                m_LatestKeyframe = t;
        }

        public void AddTextBoxKeyframe(float t)
        {
            m_AnimationClip.AddTextBoxKeyframe(t);

            if (t > m_LatestKeyframe)
                m_LatestKeyframe = t;
        }

        public NpcAnimationClip Build()
        {
            m_AnimationClip.m_Duration = m_LatestKeyframe;
            return m_AnimationClip;
        }
    }
}
