using BonesVr.Characters.Npcs.Animation;
using UnityEngine;

namespace BonesVr.StageControllers
{
    public class IntroController : StageController
    {
        [Header("NPC Animations")]

        [SerializeField] private NpcAnimator _bonesAnimator;
        protected NpcAnimator BonesAnimator => _bonesAnimator;

        [SerializeField] private NpcAnimationClip _bonesAnimationClip;
        protected NpcAnimationClip BonesAnimationClip => _bonesAnimationClip;

        protected override void Awake()
        {
            base.Awake();

            if (BonesAnimationClip == null)
                Debug.LogError("No animation clip set for Bones");
        }

        protected override void Start()
        {
            base.Start();

            BonesAnimator.StartClip(BonesAnimationClip, new(OnBonesAnimationClipFinished));
        }

        protected void OnBonesAnimationClipFinished()
        {
            ProgressFromStage();
        }
    }
}
