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

        [Header("Slot")]

        [SerializeField] private BoneType _correctBoneType = BoneType.Unknown;
        public BoneType CorrectBoneType => _correctBoneType;

        [Header("Preview Gizmo")]

        [SerializeField] private Mesh _previewGizmoMesh;
        protected Mesh PreviewGizmoMesh => _previewGizmoMesh;

        [Tooltip("The game object (this can be a prefab) with an XR grab interactable component for the preview. This is used to find an attach transform for the preview gizmo")]
        [SerializeField] private GameObject _previewInteractableGameObject;

        [SerializeField] private Vector3 _previewGizmoPosition = Vector3.zero;
        [SerializeField] private Quaternion _previewGizmoRotation = Quaternion.identity;
        [SerializeField] private Vector3 _previewGizmoScale = Vector3.one;

        [Header("Status Indicator")]

        [SerializeField] private GameObject _statusIndicatorCorrect;
        protected GameObject StatusIndicatorCorrect => _statusIndicatorCorrect;

        [SerializeField] private GameObject _statusIndicatorIncorrect;
        protected GameObject StatusIndicatorIncorrect => _statusIndicatorIncorrect;

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

        protected Matrix4x4 GetPreviewGizmoTrsMat()
            => InteractorTrsMat * GetPreviewInteractableAttachTrsMat() * GetGizmoExtraTrsMat();

        protected TRS GetPreviewGizmoTrs()
            => TransformMatrices.ToTrs(GetPreviewGizmoTrsMat());

        protected virtual void Start()
        {
            HideStatusIndicators();
        }

        protected virtual void OnEnable()
        {
            Interactor.selectEntered.AddListener(OnInteractorSelectEnter);
            Interactor.selectExited.AddListener(OnInteractorSelectExit);
        }

        protected virtual void OnDisable()
        {
            Interactor.selectEntered.RemoveListener(OnInteractorSelectEnter);
            Interactor.selectExited.RemoveListener(OnInteractorSelectExit);
        }

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

        private void OnInteractorSelectEnter(SelectEnterEventArgs args)
        {
            if (args.interactableObject.transform.TryGetComponent<BoneArrangementBone>(out var bone))
                OnBoneSelected(bone);
        }

        private void OnInteractorSelectExit(SelectExitEventArgs _)
        {
            HideStatusIndicators();
        }

        protected virtual void OnBoneSelected(BoneArrangementBone bone)
        {
            if (bone.Type == CorrectBoneType)
                ShowCorrectStatusIndicator();
            else
                ShowIncorrectStatusIndicator();

            if (bone.Type == CorrectBoneType)
            {
                // TODO - this is where the correct bone has been selected. Can do something here
                // TODO - this seems to be called repeatedly when the correct bone is in the slot, not just when it starts being selected. Might be a problem
            }
        }

        private void ShowCorrectStatusIndicator()
        {
            StatusIndicatorCorrect.SetActive(true);
            StatusIndicatorIncorrect.SetActive(false);
        }

        private void ShowIncorrectStatusIndicator()
        {
            StatusIndicatorCorrect.SetActive(false);
            StatusIndicatorIncorrect.SetActive(true);
        }

        private void HideStatusIndicators()
        {
            StatusIndicatorCorrect.SetActive(false);
            StatusIndicatorIncorrect.SetActive(false);
        }
    }
}
