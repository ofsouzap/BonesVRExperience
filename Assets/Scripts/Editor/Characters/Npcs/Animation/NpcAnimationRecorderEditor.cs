using BonesVr.Characters.Npcs.Animation;
using UnityEditor;
using UnityEngine;

namespace BonesVr.Editor.Characters.Npcs.Animation
{
    [CustomEditor(typeof(NpcAnimationRecorder))]
    public class NpcAnimationRecorderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            NpcAnimationRecorder targetR = target as NpcAnimationRecorder;

            GUI.enabled = Application.isPlaying && !targetR.IsRecording;
            if (GUILayout.Button("Start Recording"))
                targetR.StartRecording();

            GUI.enabled = Application.isPlaying && targetR.IsRecording;
            if (GUILayout.Button("Stop Recording"))
                targetR.StopRecording();

            GUI.enabled = true;
        }
    }
}
