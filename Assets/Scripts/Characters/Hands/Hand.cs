using UnityEngine;

namespace BonesVr.Characters.Hands
{
    public class Hand : MonoBehaviour
    {
        private const string AnimParamThumbTouched = "ThumbTouched";
        private const string AnimParamIndexTouched = "IndexTouched";
        private const string AnimParamGripTouched = "GripTouched";
        private const string AnimParamThumbVal = "ThumbVal";
        private const string AnimParamIndexVal = "IndexVal";
        private const string AnimParamGripVal = "GripVal";

        [SerializeField] private Animator _animator;
        private Animator Animator => _animator;
        
        protected virtual void Awake()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
            if (_animator == null)
                Debug.LogWarning("No animator set or found");
        }

        public void SetThumbTouched(bool touched) => Animator.SetBool(AnimParamThumbTouched, touched);
        public void SetIndexTouched(bool touched) => Animator.SetBool(AnimParamIndexTouched, touched);
        public void SetGripTouched(bool touched) => Animator.SetBool(AnimParamGripTouched, touched);
        public void SetThumbVal(float val) => Animator.SetFloat(AnimParamThumbVal, val);
        public void SetIndexVal(float val) => Animator.SetFloat(AnimParamIndexVal, val);
        public void SetGripVal(float val) => Animator.SetFloat(AnimParamGripVal, val);
    }
}
