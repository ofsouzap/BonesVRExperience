using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BonesVr.Minigames.Arrangement
{
    public class XRSocketInteractor_BoneArrangement : XRSocketInteractor
    {
        private bool MinigameBoneIsHoverCompatible(BoneArrangementBone minigameBone)
            => !minigameBone.IsSocketHeld;

        protected bool TransformIsHoverCompatible(Transform transform)
            => transform.TryGetComponent<BoneArrangementBone>(out var minigameBone) && MinigameBoneIsHoverCompatible(minigameBone);

        protected bool TransformIsSelectCompatible(Transform transform)
            => transform.TryGetComponent<BoneArrangementBone>(out var _);

        public override bool CanHover(IXRHoverInteractable interactable)
            => base.CanHover(interactable) && TransformIsHoverCompatible(interactable.transform);

        public override bool CanSelect(IXRSelectInteractable interactable)
            => base.CanSelect(interactable) && TransformIsSelectCompatible(interactable.transform);
    }
}
