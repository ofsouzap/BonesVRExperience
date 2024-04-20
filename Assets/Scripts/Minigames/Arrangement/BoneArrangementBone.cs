using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BonesVr.Minigames.Arrangement
{
    [RequireComponent(typeof(Bone))]
    [RequireComponent(typeof(XRBaseInteractable))]
    public class BoneArrangementBone : MonoBehaviour
    {
        public Bone Bone => GetComponent<Bone>();

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
            Bone.SocketHoldStart.AddListener(OnSocketHeldStart);
            Bone.SocketHoldEnd.AddListener(OnSocketHeldEnd);
        }

        protected virtual void OnDisable()
        {
            Bone.SocketHoldStart.RemoveListener(OnSocketHeldStart);
            Bone.SocketHoldEnd.RemoveListener(OnSocketHeldEnd);
        }

        private void OnSocketHeldStart(XRSocketInteractor _) => IsSocketHeld = true;
        private void OnSocketHeldEnd(XRSocketInteractor _) => IsSocketHeld = false;
    }
}
