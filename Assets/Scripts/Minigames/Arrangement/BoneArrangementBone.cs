using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BonesVr.Minigames.Arrangement
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class BoneArrangementBone : MonoBehaviour
    {
        public XRBaseInteractable Interactable => GetComponent<XRBaseInteractable>();

        /// <summary>
        /// Whether a socket is currently holding this bone.
        /// </summary>
        public bool IsSocketHeld { get; private set; }

        protected virtual void Start()
        {
            IsSocketHeld = false;
        }

        protected virtual void OnEnable()
        {
            Interactable.selectEntered.AddListener(OnSelected);
            Interactable.selectExited.AddListener(OnDeselected);
        }

        protected virtual void OnDisable()
        {
            Interactable.selectEntered.RemoveListener(OnSelected);
            Interactable.selectExited.RemoveListener(OnDeselected);
        }

        private void OnSelected(SelectEnterEventArgs args)
        {
            if (args.interactorObject is XRSocketInteractor)
            {
                IsSocketHeld = true;
            }
        }

        private void OnDeselected(SelectExitEventArgs args)
        {
            if (args.interactorObject is XRSocketInteractor)
            {
                IsSocketHeld = false;
            }
        }
    }
}
