using UnityEngine;

namespace BonesVr.Progression
{
    [CreateAssetMenu(fileName = "PlayerProgress", menuName = "Progression/PlayerProgress")]
    public class PlayerProgress : ScriptableObject
    {
        [SerializeField] private ProgressionStage m_CurrentStage;

        public ProgressionStage GetCurrentStage() => m_CurrentStage;
    }
}
