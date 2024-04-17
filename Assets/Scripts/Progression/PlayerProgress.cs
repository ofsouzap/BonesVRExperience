using UnityEngine;
using UnityEngine.Events;

namespace BonesVr.Progression
{
    [CreateAssetMenu(fileName = "PlayerProgress", menuName = "Progression/PlayerProgress")]
    public class PlayerProgress : ScriptableObject
    {
        [SerializeField] private ProgressionStage m_CurrentStage;

        public UnityEvent OnStageChanged = new();

        public ProgressionStage GetCurrentStage() => m_CurrentStage;

        private void SetCurrentStage(ProgressionStage stage)
        {
            m_CurrentStage = stage;
            OnStageChanged.Invoke();
        }

        /// <summary>
        /// Have the player progress forwards one stage.
        /// If the current stage has no next state set then this will log an error message to the console and return false.
        /// </summary>
        public bool ProgressStage()
        {
            if (GetCurrentStage().GetNextStage() != null)
            {
                SetCurrentStage(GetCurrentStage().GetNextStage());
                return true;
            }
            else
            {
                Debug.LogError("Trying to progress stage when current stage has no next stage");
                return false;
            }
        }
    }
}
