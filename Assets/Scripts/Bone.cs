using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace BonesVr
{
    public enum BoneType
    {
        // NOTE: When editing, so that the serialized fields in the Unity game objects aren't broken, don't ever reuse the enum indexes.
        //       E.g. I added LeftRadius after RightHumerus so it has a greater enum index than it but I declare it first for neatness reasons.
        Unknown = 0, // This is only really so that I have a default option. It shouldn't every really be used
        Skull = 1,
        Mandible = 2,
        LeftHumerus = 3,
        LeftRadius = 8,
        LeftUlna = 9,
        RightHumerus = 4,
        RightRadius = 10,
        RightUlna = 11,
        VertebraUpper = 5,
        VertebraMid = 6,
        VertebraLower = 7,
        LeftFemur = 12,
        RightFemur = 13,
    }

    public class Bone : MonoBehaviour
    {
        [SerializeField] private BoneType _boneType = BoneType.Unknown;
        public BoneType Type => _boneType;

        [Header("XR Interaction")]

        [SerializeField] private XRGrabInteractable _xrGrabInteractable;
        /// <summary>
        /// An XR grab interactable for this bone.
        /// Note that this might be null.
        /// </summary>
        protected XRGrabInteractable XrGrabInteractable => _xrGrabInteractable;

        [Tooltip("Triggers when the bone starts being held by a non-socket interactor")]
        public UnityEvent<XRBaseInteractor> PlayerHoldStart;

        [Tooltip("Triggers when the bone stops being held by a non-socket interactor")]
        public UnityEvent<XRBaseInteractor> PlayerHoldEnd;

        [Tooltip("Triggers when the bone starts being held by a socket interactor")]
        public UnityEvent<XRSocketInteractor> SocketHoldStart;

        [Tooltip("Triggers when the bone stops being held by a socket interactor")]
        public UnityEvent<XRSocketInteractor> SocketHoldEnd;

        protected virtual void Awake()
        {
            if (_xrGrabInteractable == null)
                _xrGrabInteractable = GetComponentInParent<XRGrabInteractable>();
        }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void OnValidate()
        {
            if (Type == BoneType.Unknown)
                Debug.LogWarning("No bone type set");
        }

        protected virtual void OnEnable()
        {
            if (XrGrabInteractable != null)
            {
                XrGrabInteractable.selectEntered.AddListener(OnGrabInteractableSelectEntered);
                XrGrabInteractable.selectExited.AddListener(OnGrabInteractableSelectExited);
            }
        }

        protected virtual void OnDisable()
        {
            if (XrGrabInteractable != null)
            {
                XrGrabInteractable.selectEntered.RemoveListener(OnGrabInteractableSelectEntered);
                XrGrabInteractable.selectExited.RemoveListener(OnGrabInteractableSelectExited);
            }
        }

        private void OnGrabInteractableSelectEntered(SelectEnterEventArgs args)
        {
            if (args.interactorObject is XRSocketInteractor socketInteractor)
            {
                SocketHoldStart.Invoke(socketInteractor);
            }
            else
            {
                PlayerHoldStart.Invoke(args.interactorObject as XRBaseInteractor);
            }
        }

        private void OnGrabInteractableSelectExited(SelectExitEventArgs args)
        {
            if (args.interactorObject is XRSocketInteractor socketInteractor)
            {
                SocketHoldEnd.Invoke(socketInteractor);
            }
            else
            {
                PlayerHoldEnd.Invoke(args.interactorObject as XRBaseInteractor);
            }
        }
    }
}
