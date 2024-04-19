using UnityEngine;
using static BonesVr.Characters.Npcs.Animation.NpcAnimationClip;

namespace BonesVr.Characters.Npcs.Animation
{
    public class NpcAnimationClipBuilder
    {
        protected readonly NpcAnimationClip m_AnimationClip;

        public NpcAnimationClipBuilder()
        {
            m_AnimationClip = ScriptableObject.CreateInstance<NpcAnimationClip>();
        }

        public void AddKeyframe(float t, Snapshot snap)
            => m_AnimationClip.GiveNextSnapshot(t, snap);

        public void AddTextBoxKeyframe(float t)
            => m_AnimationClip.AddTextBoxKeyframe(t);

        public NpcAnimationClip Build()
            => m_AnimationClip;
    }
}
