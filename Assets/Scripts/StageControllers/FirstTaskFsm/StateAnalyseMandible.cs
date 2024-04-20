using BonesVr.Characters.Npcs.Animation;
using UnityEngine;

namespace BonesVr.StageControllers.FirstTaskFsm
{
    public class StageAnalyseMandible : StateBase
    {
        [SerializeField] private NpcAnimationClip _bonesAnimation;
        protected NpcAnimationClip BonesAnimation => _bonesAnimation;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            PlayNpcBonesAnimation(BonesAnimation);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            m_Controller.SetMandibleAnalysed();
        }
    }
}
