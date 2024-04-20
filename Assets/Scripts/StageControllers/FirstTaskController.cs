using BonesVr.Characters.Npcs.Animation;
using BonesVr.Minigames.Arrangement;
using BonesVr.Minigames.Cleaning;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BonesVr.StageControllers
{
    public class FirstTaskController : StageController
    {
        protected const string c_AnimParam_boolBonesAllArranged = "boolBonesAllArranged";
        protected const string c_AnimParam_boolBonesAllCleaned = "boolBonesAllCleaned";

        protected const string c_AnimParam_boolLUlnaPickedUp = "boolLUlnaPickedUp";
        protected const string c_AnimParam_boolSkullPickedUp = "boolSkullPickedUp";
        protected const string c_AnimParam_boolMandiblePickedUp = "boolMandiblePickedUp";

        protected const string c_AnimParam_boolLUlnaAnalyzed = "boolLUlnaAnalyzed";
        protected const string c_AnimParam_boolSkullAnalyzed = "boolSkullAnalyzed";
        protected const string c_AnimParam_boolMandibleAnalyzed = "boolMandibleAnalyzed";

        protected const string c_AnimParam_triggerAnimationCompleted = "triggerAnimCompleted";

        [Header("State Machine")]

        [SerializeField] private Animator _stateMachine;
        protected Animator StateMachine => _stateMachine;

        [Header("Arrangement")]

        [SerializeField] private ArrangementMinigame _arrangementMinigame;
        protected ArrangementMinigame ArrangementMinigame => _arrangementMinigame;

        [Header("NPCs")]

        [SerializeField] private NpcAnimator _npcBonesAnimator;
        protected NpcAnimator NpcBonesAnimator => _npcBonesAnimator;

        [Header("Bones")]

        [SerializeField] private Bone _lUlna;
        protected Bone LUlna => _lUlna;

        [SerializeField] private Bone _skull;
        protected Bone Skull => _skull;

        [SerializeField] private Bone _mandible;
        protected Bone Mandible => _mandible;

        [SerializeField] private DirtyBoneController[] _dirtyBones;
        protected DirtyBoneController[] DirtyBones => _dirtyBones ?? Array.Empty<DirtyBoneController>();

        protected bool LUlnaPickedUp { get; private set; }
        protected bool SkullPickedUp { get; private set; }
        protected bool MandiblePickedUp { get; private set; }

        protected bool LUlnaAnalysed { get; private set; }
        protected bool SkullAnalysed { get; private set; }
        protected bool MandibleAnalysed { get; private set; }

        protected override void Update()
        {
            base.Update();

            UpdateFsmParameters();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            NpcBonesAnimator.m_AnyClipFinished.AddListener(OnNpcBonesAnyClipFinished);

            LUlna.PlayerHoldStart.AddListener(OnLULnaHoldStart);
            LUlna.PlayerHoldEnd.AddListener(OnLULnaHoldEnd);
            Skull.PlayerHoldStart.AddListener(OnSkullHoldStart);
            Skull.PlayerHoldEnd.AddListener(OnSkullHoldEnd);
            Mandible.PlayerHoldStart.AddListener(OnMandibleHoldStart);
            Mandible.PlayerHoldEnd.AddListener(OnMandibleHoldEnd);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            NpcBonesAnimator.m_AnyClipFinished.RemoveListener(OnNpcBonesAnyClipFinished);

            LUlna.PlayerHoldStart.RemoveListener(OnLULnaHoldStart);
            LUlna.PlayerHoldEnd.RemoveListener(OnLULnaHoldEnd);
            Skull.PlayerHoldStart.RemoveListener(OnSkullHoldStart);
            Skull.PlayerHoldEnd.RemoveListener(OnSkullHoldEnd);
            Mandible.PlayerHoldStart.RemoveListener(OnMandibleHoldStart);
            Mandible.PlayerHoldEnd.RemoveListener(OnMandibleHoldEnd);
        }

        protected void UpdateFsmParameters()
        {
            StateMachine.SetBool(c_AnimParam_boolBonesAllCleaned, DirtyBones.All(x => x.IsFullyClean));
            StateMachine.SetBool(c_AnimParam_boolBonesAllArranged, ArrangementMinigame.MinigameInCompletedState);
            StateMachine.SetBool(c_AnimParam_boolLUlnaAnalyzed, LUlnaAnalysed);
            StateMachine.SetBool(c_AnimParam_boolSkullAnalyzed, SkullAnalysed);
            StateMachine.SetBool(c_AnimParam_boolMandibleAnalyzed, MandibleAnalysed);
            StateMachine.SetBool(c_AnimParam_boolLUlnaPickedUp, LUlnaPickedUp);
            StateMachine.SetBool(c_AnimParam_boolSkullPickedUp, SkullPickedUp);
            StateMachine.SetBool(c_AnimParam_boolMandiblePickedUp, MandiblePickedUp);
        }

        private void OnNpcBonesAnyClipFinished(NpcAnimationClip _) => SetAnimationCompleted();

        private void OnLULnaHoldStart(XRBaseInteractor _) => LUlnaPickedUp = true;
        private void OnLULnaHoldEnd(XRBaseInteractor _) => LUlnaPickedUp = false;
        private void OnSkullHoldStart(XRBaseInteractor _) => SkullPickedUp = true;
        private void OnSkullHoldEnd(XRBaseInteractor _) => SkullPickedUp = false;
        private void OnMandibleHoldStart(XRBaseInteractor _) => MandiblePickedUp = true;
        private void OnMandibleHoldEnd(XRBaseInteractor _) => MandiblePickedUp = false;

        protected void SetAnimationCompleted() => StateMachine.SetTrigger(c_AnimParam_triggerAnimationCompleted);
        public void ResetAnimationCompleted() => StateMachine.ResetTrigger(c_AnimParam_triggerAnimationCompleted);

        public void SetLUlnaAnalysed() => LUlnaAnalysed = true;
        public void SetSkullAnalysed() => SkullAnalysed = true;
        public void SetMandibleAnalysed() => MandibleAnalysed = true;

        public void PlayNpcBonesAnimation(NpcAnimationClip clip)
        {
            if (clip != null)
                NpcBonesAnimator.StartClip(clip);
            else
                Debug.LogError("No clip provided");
        }
    }
}
