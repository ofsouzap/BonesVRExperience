using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using BonesVr.Utils;

namespace BonesVr.Minigames.Arrangement
{
    [RequireComponent(typeof(XRSocketInteractor_BoneArrangement))]
    public class BoneArrangementSocketController : MonoBehaviour
    {
        public XRSocketInteractor Interactor => GetComponent<XRSocketInteractor>();

        private TRS InteractorAttachTrs => TRS.FromTransform(Interactor.attachTransform.transform);
        private TRS InteractorTransformTrs => TRS.FromTransform(Interactor.transform);

        protected TRS InteractorTrs => Interactor.attachTransform != null ? InteractorAttachTrs : InteractorTransformTrs;
        protected Matrix4x4 InteractorTrsMat => TransformMatrices.FromTrs(InteractorTrs);

        //protected Vector3 InteractorAttachPosition => Interactor.attachTransform != null ? Interactor.attachTransform.position : transform.position;
        //protected Quaternion InteractorAttachRotation => Interactor.attachTransform != null ? Interactor.attachTransform.rotation : transform.rotation;
        //protected Vector3 InteractorAttachScale => Interactor.attachTransform != null ? Interactor.attachTransform.localScale : transform.localScale;

        [Header("Preview Gizmo")]

        [SerializeField] private Mesh _previewGizmoMesh;
        protected Mesh PreviewGizmoMesh => _previewGizmoMesh;

        [Tooltip("The game object (this can be a prefab) with an XR grab interactable component for the preview. This is used to find an attach transform for the preview gizmo")]
        [SerializeField] private GameObject _previewInteractableGameObject;

        [SerializeField] private Vector3 _previewGizmoPosition = Vector3.zero;
        [SerializeField] private Quaternion _previewGizmoRotation = Quaternion.identity;
        [SerializeField] private Vector3 _previewGizmoScale = Vector3.one;

        protected Matrix4x4 GetGizmoExtraTrsMat()
            => TransformMatrices.FromTrs(_previewGizmoPosition, _previewGizmoRotation, _previewGizmoScale);

        protected XRGrabInteractable GetPreviewInteractable()
        {
            if (_previewInteractableGameObject == null)
                return null;
            else
                return _previewInteractableGameObject.GetComponent<XRGrabInteractable>();
        }

        protected Matrix4x4 GetPreviewInteractableAttachTrsMat()
        {
            XRGrabInteractable interactable = GetPreviewInteractable();

            if (interactable == null || interactable.attachTransform == null)
                return Matrix4x4.identity;
            else
            {
                Matrix4x4 attachMat = TransformMatrices.FromTransform(interactable.attachTransform.transform);
                Matrix4x4 interactableMat = TransformMatrices.FromTransform(interactable.transform);
                return interactableMat * attachMat.inverse;
            }
        }

        //protected Vector3 GetInteractableAttachPosition()
        //{
        //    XRGrabInteractable interactable = GetPreviewInteractable();

        //    if (interactable == null || interactable.attachTransform == null)
        //        return Vector3.zero;
        //    else
        //        return interactable.transform.position - interactable.attachTransform.position;

        //}

        //protected Quaternion GetInteractableAttachRotation()
        //{
        //    XRGrabInteractable interactable = GetPreviewInteractable();

        //    if (interactable == null || interactable.attachTransform == null)
        //        return Quaternion.identity;
        //    else
        //        return interactable.transform.rotation * Quaternion.Inverse(interactable.attachTransform.rotation);
        //}

        //protected Vector3 GetInteractableAttachScale()
        //{
        //    XRGrabInteractable interactable = GetPreviewInteractable();

        //    if (interactable == null || interactable.attachTransform == null)
        //        return Vector3.one;
        //    else
        //        return new(
        //            interactable.transform.lossyScale.x / interactable.attachTransform.lossyScale.x,
        //            interactable.transform.lossyScale.y / interactable.attachTransform.lossyScale.y,
        //            interactable.transform.lossyScale.z / interactable.attachTransform.lossyScale.z
        //        );
        //}

        //protected Vector3 GetGizmoPosition() => InteractorAttachPosition + GetInteractableAttachPosition() + _previewGizmoPosition;
        //protected Quaternion GetGizmoRotation() => InteractorAttachRotation * GetInteractableAttachRotation() * _previewGizmoRotation;
        //protected Vector3 GetGizmoScale()
        //{
        //    Vector3 scale = _previewGizmoScale;
        //    scale.Scale(InteractorAttachScale);
        //    scale.Scale(GetInteractableAttachScale());
        //    return scale;
        //}

        protected Matrix4x4 GetPreviewGizmoTrsMat()
            => InteractorTrsMat * GetPreviewInteractableAttachTrsMat() * GetGizmoExtraTrsMat();

        protected TRS GetPreviewGizmoTrs()
            => TransformMatrices.ToTrs(GetPreviewGizmoTrsMat());

        protected virtual void OnValidate()
        {
            if (_previewInteractableGameObject != null && GetPreviewInteractable() == null)
                Debug.LogError("Preview interactable game object has no XR grab interactable component");
        }

        protected virtual void OnDrawGizmos()
        {
            if (PreviewGizmoMesh != null)
            {
                TRS trs = GetPreviewGizmoTrs();
                Gizmos.DrawWireMesh(PreviewGizmoMesh, trs.position, trs.rotation, trs.scale);
            }
        }
    }
}
