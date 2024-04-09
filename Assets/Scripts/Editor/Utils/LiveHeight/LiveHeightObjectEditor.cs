using BonesVr.Utils.LiveHeight;
using UnityEditor;

namespace BonesVr.Editor.Utils.LiveHeight
{
    [CustomEditor(typeof(LiveHeightObject))]
    [CanEditMultipleObjects]
    public class LiveHeightObjectEditor : UnityEditor.Editor
    {
        private SerializedProperty m_PropLiveMinHeight;
        private LiveHeightObject Target => target as LiveHeightObject;

        private void OnEnable()
        {
            m_PropLiveMinHeight = serializedObject.FindProperty("_liveMinHeight");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (!Target.UsesGrouping)
                EditorGUILayout.PropertyField(m_PropLiveMinHeight);
            else
                EditorGUILayout.LabelField($"(Uses live height group)");
            serializedObject.ApplyModifiedProperties();
        }
    }
}
