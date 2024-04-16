using UnityEngine;
using UnityEngine.Events;

namespace BonesVr.Minigames.Cleaning
{
    [RequireComponent(typeof(Collider))]
    public class CleaningBrushTriggerController : MonoBehaviour
    {
        protected Collider Collider => GetComponent<Collider>();

        public UnityEvent<DirtyBoneController> DirtyBoneEnteredTrigger = new();

        protected virtual void OnValidate()
        {
            if (!Collider.isTrigger)
            {
                Debug.LogError("Collider not set as trigger. Setting collider as trigger.");
                Collider.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            DirtyBoneController dirtyBone = other.GetComponentInParent<DirtyBoneController>();
            if (dirtyBone != null)
                DirtyBoneEnteredTrigger.Invoke(dirtyBone);
        }
    }
}
