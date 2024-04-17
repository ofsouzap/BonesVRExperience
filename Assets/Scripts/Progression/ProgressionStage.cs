using UnityEngine;

namespace BonesVr.Progression
{
    [CreateAssetMenu(fileName = "ProgressStage", menuName = "Progression/ProgressStage")]
    public class ProgressionStage : ScriptableObject
    {
        [SerializeField] private string m_StageName;
        [SerializeField] private GameObject m_MainPlatformStagePrefab;
        [SerializeField] private ProgressionStage m_NextStage;

        public string GetStageName() => m_StageName;
        public GameObject GetMainPlatformStagePrefab() => m_MainPlatformStagePrefab;
        public ProgressionStage GetNextStage() => m_NextStage;
    }
}
