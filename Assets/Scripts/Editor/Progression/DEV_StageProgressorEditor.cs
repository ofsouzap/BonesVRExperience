using UnityEngine;
using UnityEditor;
using BonesVr.Progression;

namespace BonesVr.Editor.Progression
{
    [CustomEditor(typeof(DEV_StageProgressor))]
    public class DEV_StageProgressorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Progress One Stage"))
                (target as DEV_StageProgressor).ProgressStage();
        }
    }
}
