using UnityEngine;

namespace BonesVr.Progression
{
    [CreateAssetMenu(fileName = "ProgressStage", menuName = "Progression/ProgressStage")]
    public class ProgressionStage : ScriptableObject
    {
        [SerializeField] private string m_StageName;
        [SerializeField] private ProgressionStage m_NextStage;

        [SerializeField] private bool m_UsePlayerStartPosition = false;
        [SerializeField] private Vector3 m_PlayerStartPosition;

        [SerializeField] private bool m_UsePlayerStartRotation = false;
        [SerializeField] private Vector3 m_PlayerStartRotationEuler;

        [SerializeField] private GameObject m_MainPlatformStagePrefab;

        public string GetStageName() => m_StageName;
        public Vector3? GetPlayerStartPosition() => m_UsePlayerStartPosition ? m_PlayerStartPosition : null;
        public Quaternion? GetPlayerStartRotation() => m_UsePlayerStartRotation ? Quaternion.Euler(m_PlayerStartRotationEuler) : null;
        public GameObject GetMainPlatformStagePrefab() => m_MainPlatformStagePrefab;
        public ProgressionStage GetNextStage() => m_NextStage;
    }
}
