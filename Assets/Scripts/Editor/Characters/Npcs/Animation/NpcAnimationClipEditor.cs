using BonesVr.Characters.Npcs.Animation;
using UnityEditor;

namespace BonesVr.Editor.Characters.Npcs.Animation
{
    [CustomEditor(typeof(NpcAnimationClip))]
    public class NpcAnimationClipEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            NpcAnimationClip clip = target as NpcAnimationClip;

            EditorGUILayout.LabelField($"{clip.Keyframes.Length} keyframes");

            if (clip.Keyframes.Length > 0)
            {
                EditorGUILayout.LabelField($"Start position: ({clip.Keyframes[0].keyframe.m_RootLocalPosition.x}, {clip.Keyframes[0].keyframe.m_RootLocalPosition.y}, {clip.Keyframes[0].keyframe.m_RootLocalPosition.z})");
                int lastIdx = clip.Keyframes.Length - 1;
                EditorGUILayout.LabelField($"End position: ({clip.Keyframes[lastIdx].keyframe.m_RootLocalPosition.x}, {clip.Keyframes[lastIdx].keyframe.m_RootLocalPosition.y}, {clip.Keyframes[lastIdx].keyframe.m_RootLocalPosition.z})");
            }
        }
    }
}
