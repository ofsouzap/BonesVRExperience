using BonesVr.Characters.Npcs.Animation;
using UnityEngine;

namespace BonesVr.StageControllers.FirstTaskFsm
{
    public abstract class StateBase : StateMachineBehaviour
    {
        protected Animator m_Animator;
        protected FirstTaskController m_Controller;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            m_Animator = animator;
            if (!m_Animator.TryGetComponent(out m_Controller))
                Debug.LogError("Stage controller not found on animator");

            if (m_Controller != null)
                m_Controller.ResetAnimationCompleted(); // In case the trigger isn't used by the transition into this state
        }

        protected void PlayNpcBonesAnimation(NpcAnimationClip clip) => m_Controller.PlayNpcBonesAnimation(clip);
    }
}
