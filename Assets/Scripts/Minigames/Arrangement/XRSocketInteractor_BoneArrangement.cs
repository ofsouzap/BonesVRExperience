using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BonesVr.Minigames.Arrangement
{
    public class XRSocketInteractor_BoneArrangement : XRSocketInteractor
    {
        private bool MinigameBoneIsCompatible(BoneArrangementBone minigameBone)
            => !minigameBone.IsSocketHeld;

        protected bool TransformIsCompatible(Transform transform)
            => transform.TryGetComponent<BoneArrangementBone>(out var minigameBone) && MinigameBoneIsCompatible(minigameBone);

        public override bool CanHover(IXRHoverInteractable interactable)
            => base.CanHover(interactable) && TransformIsCompatible(interactable.transform);

        public override bool CanSelect(IXRSelectInteractable interactable)
            => base.CanSelect(interactable) && TransformIsCompatible(interactable.transform);
    }
}
