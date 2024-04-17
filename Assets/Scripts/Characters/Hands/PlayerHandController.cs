using UnityEngine;
using UnityEngine.InputSystem;

namespace BonesVr.Characters.Hands
{
    [RequireComponent(typeof(Hand))]
    public class PlayerHandController : MonoBehaviour
    {
        protected Hand Hand { get; private set; }

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
            Hand = GetComponent<Hand>();
        }

        protected virtual void Update()
        {
            Hand.SetThumbTouched(ThumbTouchedAction.ReadValue<float>() > .5f);
            Hand.SetTriggerTouched(TriggerTouchedAction.ReadValue<float>() > .5f);
            Hand.SetGripTouched(GripTouchedAction.ReadValue<float>() > .5f);
            Hand.SetThumbVal(ThumbValAction.ReadValue<float>());
            Hand.SetTriggerVal(TriggerValAction.ReadValue<float>());
            Hand.SetGripVal(GripValAction.ReadValue<float>());
        }
    }
}
