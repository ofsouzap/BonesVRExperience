using UnityEngine;
using UnityEditor;
using BonesVr.Characters.Npcs.Animation;

namespace BonesVr.Editor.Characters.Npcs.Animation
{
    [CustomEditor(typeof(DEV_NpcAnimatorAutoplay))]
    public class DEV_NpcAnimatorAutoplayEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DEV_NpcAnimatorAutoplay targetA = target as DEV_NpcAnimatorAutoplay;

            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Start Clip"))
                targetA.StartSelectedClip();

            GUI.enabled = true;
        }
    }
}
