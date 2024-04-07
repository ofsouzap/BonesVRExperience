using UnityEngine;

namespace BonesVr.Utils
{
    public class InteractionActivatedAnimator : MonoBehaviour
    {
        [Tooltip("The animator to affect")]
        [SerializeField] private Animator _animator;
        protected Animator Animator => _animator;

        [SerializeField] private string _boolParamName;
        protected string BoolParamName => _boolParamName;

        protected virtual void Awake()
        {
            if (Animator == null)
                _animator = GetComponentInChildren<Animator>();
            if (Animator == null)
                Debug.LogWarning("No animator set or found");
        }

        public void EnableToggle()
        {
            Animator.SetBool(BoolParamName, true);
        }

        public void DisableToggle()
        {
            Animator.SetBool(BoolParamName, false);
        }
    }
}
