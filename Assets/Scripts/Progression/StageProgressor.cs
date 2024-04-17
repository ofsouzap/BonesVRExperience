using UnityEngine;

namespace BonesVr.Progression
{
    public class StageProgressor : MonoBehaviour
    {
        [SerializeField] private PlayerProgress m_PlayerProgress;

        protected virtual void Awake()
        {
            if (m_PlayerProgress == null)
                Debug.LogError("No player progress instance provided");
        }

        public void ProgressStage()
        {
            m_PlayerProgress.ProgressStage();
        }
    }
}
