using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace BonesVr.Minigames.Arrangement
{
    public class ArrangementMinigame : MonoBehaviour
    {
        /// <summary>
        /// When the minigame is first completed.
        /// </summary>
        public UnityEvent MinigameFirstCompleted;

        /// <summary>
        /// Whether the minigame has ever been completed.
        /// </summary>
        private bool m_MinigameCompletedBefore;

        /// <summary>
        /// Whether the minigame is currently in a completed state.
        /// </summary>
        public bool MinigameInCompletedState { get; private set; }

        protected virtual void Start()
        {
            m_MinigameCompletedBefore = false;
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

        public void OnMinigameCompleted()
        {
            if (!m_MinigameCompletedBefore)
                MinigameFirstCompleted.Invoke();

            m_MinigameCompletedBefore = true;
        }

        /// <summary>
        /// To be called when any of the sockets have changed what they are holding.
        /// This must be called after changes are made to the sockets' internal state.
        /// </summary>
        public void OnSocketHasChanged()
        {
            if (CheckIfMinigameComplete())
            {
                MinigameInCompletedState = true;
                OnMinigameCompleted();
            }
            else
                MinigameInCompletedState = false;
        }
    }
}
