using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    protected Animator Animator => _animator;

    private const string AnimParamThumbVal = "ThumbVal";
    private const string AnimParamIndexVal = "IndexVal";
    private const string AnimParamOtherFingersVal = "OtherFingersVal";

    [SerializeField] private InputActionReference _thumbValAction;
    protected InputAction ThumbValAction => _thumbValAction.action;

    [SerializeField] private InputActionReference _indexValAction;
    protected InputAction IndexValAction => _indexValAction.action;

    [SerializeField] private InputActionReference _otherFingersValAction;
    protected InputAction OtherFingersValAction => _otherFingersValAction.action;

    protected virtual void Awake()
    {
        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
            Debug.LogWarning("No animator set or found");
    }

    protected virtual void Update()
    {
        Animator.SetFloat(AnimParamThumbVal, ThumbValAction.ReadValue<float>());
        Animator.SetFloat(AnimParamIndexVal, IndexValAction.ReadValue<float>());
        Animator.SetFloat(AnimParamOtherFingersVal, OtherFingersValAction.ReadValue<float>());
    }
}
