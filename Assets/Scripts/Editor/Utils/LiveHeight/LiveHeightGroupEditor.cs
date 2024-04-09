using BonesVr.Utils.LiveHeight;
using UnityEditor;

namespace BonesVr.Editor.Utils.LiveHeight
{
    [CustomEditor(typeof(LiveHeightGroup))]
    [CanEditMultipleObjects]
    public class LiveHeightGroupEditor : UnityEditor.Editor
    {
        private LiveHeightGroup Target => target as LiveHeightGroup;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Group has {Target.GetGroupUsers().Length} users");
        }
    }
}
