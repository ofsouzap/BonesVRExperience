using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace BonesVr.Minigames.Arrangement
{
    public class ArrangementMinigame : MonoBehaviour
    {
        public UnityEvent MinigameCompleted;

        private bool m_MinigameCompleted;

        protected virtual void Start()
        {
            m_MinigameCompleted = false;
        }

        protected virtual void OnValidate()
        {
            if (GetComponentsInParent<ArrangementMinigame>()
                .Where(x => x != this)
                .Any())
                Debug.LogError("This component cannot be nested");
        }

        public ICollection<BoneArrangementSocketController> GetSocketControllers()
            => GetComponentsInChildren<BoneArrangementSocketController>();

        protected bool CheckIfMinigameComplete()
            => GetSocketControllers().All(x => x.CorrectBoneHeld);

        public void CompleteMinigame()
        {
            m_MinigameCompleted = true;
            MinigameCompleted.Invoke();
        }

        /// <summary>
        /// To be called when any of the sockets have changed what they are holding.
        /// This must be called after changes are made to the sockets' internal state.
        /// </summary>
        public void OnSocketHasChanged()
        {
            if (!m_MinigameCompleted)
            {
                if (CheckIfMinigameComplete())
                    CompleteMinigame();
            }
        }
    }
}
