using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BonesVr.Minigames.Arrangement
{
    [RequireComponent(typeof(XRSocketInteractor_BoneArrangement))]
    public class BoneArrangementSocketController : MonoBehaviour
    {
        public XRSocketInteractor Interactor => GetComponent<XRSocketInteractor>();

        protected Vector3 AttachPosition => Interactor.attachTransform != null ? Interactor.attachTransform.position : transform.position;
        protected Quaternion AttachRotation => Interactor.attachTransform != null ? Interactor.attachTransform.rotation : transform.rotation;
        protected Vector3 AttachScale => Interactor.attachTransform != null ? Interactor.attachTransform.localScale : transform.localScale;

        [Header("Gizmo")]

        [SerializeField] private Mesh _gizmoMesh;
        protected Mesh GizmoMesh => _gizmoMesh;

        [SerializeField] private Quaternion _gizmoRotation = Quaternion.identity;
        protected Quaternion GizmoRotation => _gizmoRotation;

        [SerializeField] private Vector3 _gizmoScale = Vector3.one;
        protected Vector3 GizmoScale => _gizmoScale;

        protected virtual void OnDrawGizmos()
        {
            if (GizmoMesh != null)
            {
                Vector3 scale = AttachScale;
                scale.Scale(GizmoScale);
                Gizmos.DrawWireMesh(GizmoMesh, AttachPosition, AttachRotation * GizmoRotation, scale);
            }
        }
    }
}
