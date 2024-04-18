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

        public bool ThumbTouched { get; private set; }
        public bool IndexTouched { get; private set; }
        public bool GripTouched { get; private set; }
        public float ThumbVal { get; private set; }
        public float IndexVal { get; private set; }
        public float GripVal { get; private set; }

        protected virtual void Awake()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
            if (_animator == null)
                Debug.LogWarning("No animator set or found");
        }

        public void SetThumbTouched(bool touched)
        {
            ThumbTouched = touched;
            Animator.SetBool(AnimParamThumbTouched, touched);
        }

        public void SetIndexTouched(bool touched)
        {
            IndexTouched = touched;
            Animator.SetBool(AnimParamIndexTouched, touched);
        }

        public void SetGripTouched(bool touched)
        {
            GripTouched = touched;
            Animator.SetBool(AnimParamGripTouched, touched);
        }

        public void SetThumbVal(float val)
        {
            ThumbVal = val;
            Animator.SetFloat(AnimParamThumbVal, val);
        }

        public void SetIndexVal(float val)
        {
            IndexVal = val;
            Animator.SetFloat(AnimParamIndexVal, val);
        }

        public void SetGripVal(float val)
        {
            GripVal = val;
            Animator.SetFloat(AnimParamGripVal, val);
        }
    }
}
