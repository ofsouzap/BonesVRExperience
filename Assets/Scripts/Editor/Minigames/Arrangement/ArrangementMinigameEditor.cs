using UnityEditor;
using BonesVr.Minigames.Arrangement;

namespace BonesVr.Editor.Minigames.Arrangement
{
    [CustomEditor(typeof(ArrangementMinigame))]
    public class ArrangementMinigameEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ArrangementMinigame targetM = target as ArrangementMinigame;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField($"Connected sockets: {targetM.GetSocketControllers().Count}");
        }
    }
}
