using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    protected Animator Animator => _animator;

    private const string AnimParamThumbTouched = "ThumbTouched";
    private const string AnimParamTriggerTouched = "TriggerTouched";
    private const string AnimParamGripTouched = "GripTouched";
    private const string AnimParamThumbVal = "ThumbVal";
    private const string AnimParamTriggerVal = "TriggerVal";
    private const string AnimParamGripVal = "GripVal";

    [SerializeField] private InputActionReference _thumbTouchedAction;
    protected InputAction ThumbTouchedAction => _thumbTouchedAction.action;

    [SerializeField] private InputActionReference _triggerTouchedAction;
    protected InputAction TriggerTouchedAction => _triggerTouchedAction.action;

    [SerializeField] private InputActionReference _gripTouchedAction;
    protected InputAction GripTouchedAction => _gripTouchedAction.action;


    [SerializeField] private InputActionReference _thumbValAction;
    protected InputAction ThumbValAction => _thumbValAction.action;

    [SerializeField] private InputActionReference _triggerValAction;
    protected InputAction TriggerValAction => _triggerValAction.action;

    [SerializeField] private InputActionReference _gripValAction;
    protected InputAction GripValAction => _gripValAction.action;

    protected virtual void Awake()
    {
        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
            Debug.LogWarning("No animator set or found");
    }

    protected virtual void Update()
    {
        Animator.SetBool(AnimParamThumbTouched, ThumbTouchedAction.ReadValue<float>() > .5f);
        Animator.SetBool(AnimParamTriggerTouched, TriggerTouchedAction.ReadValue<float>() > .5f);
        Animator.SetBool(AnimParamGripTouched, GripTouchedAction.ReadValue<float>() > .5f);
        Animator.SetFloat(AnimParamThumbVal, ThumbValAction.ReadValue<float>());
        Animator.SetFloat(AnimParamTriggerVal, TriggerValAction.ReadValue<float>());
        Animator.SetFloat(AnimParamGripVal, GripValAction.ReadValue<float>());
    }
}
