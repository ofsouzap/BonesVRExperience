using BonesVr.Characters.Npcs.Animation;
using UnityEngine;

namespace BonesVr.StageControllers.FirstTaskFsm
{
    public class StageAnalyseInstruct : StateBase
    {
        [SerializeField] private NpcAnimationClip _bonesAnimation;
        protected NpcAnimationClip BonesAnimation => _bonesAnimation;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            PlayNpcBonesAnimation(BonesAnimation);
        }
    }
}
