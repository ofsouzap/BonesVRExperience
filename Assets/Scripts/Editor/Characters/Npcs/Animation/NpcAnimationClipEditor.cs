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

            if (clip.m_RootLocalPosition.GetKeyframes().Count > 0)
            {
                EditorGUILayout.LabelField($"Start position: ({clip.m_RootLocalPosition.GetKeyframes()[0].val.x}, {clip.m_RootLocalPosition.GetKeyframes()[0].val.y}, {clip.m_RootLocalPosition.GetKeyframes()[0].val.z})");
                EditorGUILayout.LabelField($"End position: ({clip.m_RootLocalPosition.GetKeyframes()[^1].val.x}, {clip.m_RootLocalPosition.GetKeyframes()[^1].val.y}, {clip.m_RootLocalPosition.GetKeyframes()[^1].val.z})");
            }
        }
    }
}
