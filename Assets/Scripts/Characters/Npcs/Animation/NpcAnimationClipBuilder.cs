using System;
using System.Collections.Generic;
using System.Linq;
using static BonesVr.Characters.Npcs.Animation.NpcAnimationClip;

namespace BonesVr.Characters.Npcs.Animation
{
    public class NpcAnimationClipBuilder
    {
        protected readonly IList<KeyframePair> m_Keyframes;

        public NpcAnimationClipBuilder()
        {
            m_Keyframes = new List<KeyframePair>();
        }

        public void AddKeyframe(float t, Keyframe kf)
        {
            m_Keyframes.Add(new(t, kf));
        }

        public NpcAnimationClip Build()
            => CreateFromKeyframes(m_Keyframes.ToArray());
    }
}
