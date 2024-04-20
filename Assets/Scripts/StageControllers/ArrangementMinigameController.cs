using BonesVr.Characters.Npcs.Animation;
using BonesVr.Minigames.Arrangement;
using UnityEngine;

namespace BonesVr.StageControllers
{
    public class ArrangementMinigameController : StageController
    {
        [Header("NPC Animations")]

        [SerializeField] private NpcAnimator _bonesAnimator;
        protected NpcAnimator BonesAnimator => _bonesAnimator;

        [SerializeField] private NpcAnimationClip _bonesAnimationClip;
        protected NpcAnimationClip BonesAnimationClip => _bonesAnimationClip;

        [Header("Arrangement Minigame")]

        [SerializeField] private ArrangementMinigame _arrangementMinigame;
        protected ArrangementMinigame ArrangementMinigame => _arrangementMinigame;

        [Header("Progression")]

        [SerializeField] private GameObject _progressionGate;
        protected GameObject ProgressionGate => _progressionGate;

        [SerializeField] private GameObject _progressionGateBarrier;
        protected GameObject ProgressionGateBarrier => _progressionGateBarrier;

        protected override void Awake()
        {
            base.Awake();

            if (BonesAnimationClip == null)
                Debug.LogError("No animation clip set for Bones");

            if (ArrangementMinigame == null)
                Debug.LogError("No arrangement minigame provided");

            if (ProgressionGate == null)
                Debug.LogError("No progression gate provided");
        }

        protected override void Start()
        {
            base.Start();

            LockProgressionGate();
            BonesAnimator.StartClip(BonesAnimationClip);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            ArrangementMinigame.MinigameFirstCompleted.AddListener(OnArrangementMinigameCompleted);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            ArrangementMinigame.MinigameFirstCompleted.RemoveListener(OnArrangementMinigameCompleted);
        }

        private void OnArrangementMinigameCompleted()
        {
            UnlockProgressionGate();
        }

        private void LockProgressionGate()
        {
            ProgressionGate.SetActive(false);
            if (ProgressionGateBarrier != null)
                ProgressionGateBarrier.SetActive(true);
        }

        private void UnlockProgressionGate()
        {
            ProgressionGate.SetActive(true);
            if (ProgressionGateBarrier != null)
                ProgressionGateBarrier.SetActive(false);
        }
    }
}
