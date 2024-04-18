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

        public bool ThumbTouched { get; private set; }
        public bool TriggerTouched { get; private set; }
        public bool GripTouched { get; private set; }
        public float ThumbVal { get; private set; }
        public float TriggerVal { get; private set; }
        public float GripVal { get; private set; }

        protected virtual void Awake()
        {
            Hand = GetComponent<Hand>();
        }

        protected virtual void Update()
        {
            UpdateStoredActionVals();
            UpdateHandValues();
        }

        protected void UpdateStoredActionVals()
        {
            ThumbTouched = ThumbTouchedAction.ReadValue<float>() > .5f;
            TriggerTouched = TriggerTouchedAction.ReadValue<float>() > .5f;
            GripTouched = GripTouchedAction.ReadValue<float>() > .5f;
            ThumbVal = ThumbValAction.ReadValue<float>();
            TriggerVal = TriggerValAction.ReadValue<float>();
            GripVal = GripValAction.ReadValue<float>();
        }

        protected void UpdateHandValues()
        {
            Hand.SetThumbTouched(ThumbTouched);
            Hand.SetIndexTouched(TriggerTouched);
            Hand.SetGripTouched(GripTouched);
            Hand.SetThumbVal(ThumbVal);
            Hand.SetIndexVal(TriggerVal);
            Hand.SetGripVal(GripVal);
        }
    }
}
