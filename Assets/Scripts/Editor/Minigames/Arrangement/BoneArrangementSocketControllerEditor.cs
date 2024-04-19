using BonesVr.Minigames.Arrangement;
using UnityEditor;

namespace BonesVr.Editor.Minigames.Arrangement
{
    [CustomEditor(typeof(BoneArrangementSocketController))]
    public class BoneArrangementSocketControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            BoneArrangementSocketController targetS = target as BoneArrangementSocketController;

            EditorGUILayout.Space();

            ArrangementMinigame minigameComponent = targetS.GetMinigameComponent();
            if (minigameComponent != null)
                EditorGUILayout.LabelField($"Connected minigame component: {minigameComponent.gameObject.name}");
            else
                EditorGUILayout.LabelField($"No connected minigame component!");
        }
    }
}
