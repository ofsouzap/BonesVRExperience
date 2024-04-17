using UnityEngine;
using UnityEditor;
using BonesVr.Development.AnimationRecording;

namespace BonesVr.Editor.Development.AnimationRecording
{
    [CustomEditor(typeof(NpcAnimationRecorder))]
    public class NpcAnimationRecorderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            NpcAnimationRecorder targetRecorder = target as NpcAnimationRecorder;

            DrawDefaultInspector();

            GUI.enabled = Application.isPlaying && !targetRecorder.IsRecording;
            if (GUILayout.Button("Start Recording"))
                targetRecorder.StartRecording();

            GUI.enabled = Application.isPlaying && targetRecorder.IsRecording;
            if (GUILayout.Button("Stop Recording"))
                targetRecorder.StopRecording();

            GUI.enabled = true;
        }
    }
}
