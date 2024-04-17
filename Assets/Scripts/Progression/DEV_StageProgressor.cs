using UnityEngine;

namespace BonesVr.Progression
{
    [CreateAssetMenu(fileName = "DEV_StageProgressor", menuName = "Progression/DEV_StageProgressor")]
    public class DEV_StageProgressor : ScriptableObject
    {
        [SerializeField] private PlayerProgress m_PlayerProgress;

        public void ProgressStage()
        {
            if (m_PlayerProgress.ProgressStage())
                Debug.Log($"Progressed one stage to {m_PlayerProgress.GetCurrentStage().GetStageName()}");
        }
    }
}
