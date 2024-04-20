using BonesVr.Characters.Npcs.Animation;
using UnityEditor;
using UnityEngine;

namespace BonesVr.Editor.Characters.Npcs.Animation
{
    [CustomEditor(typeof(NpcAnimator))]
    public class NpcAnimatorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            NpcAnimator targetA = target as NpcAnimator;

            if (Application.isPlaying)
            {
                if (targetA.IsPlaying)
                    EditorGUILayout.LabelField($"Clip play time: {targetA.ClipPlayTime}");
                else
                    EditorGUILayout.LabelField("No clip currently playing");
            }
        }
    }
}
