using BonesVr.Player;
using UnityEngine;
using UnityEngine.Events;

namespace BonesVr.Utils
{
    [RequireComponent(typeof(Collider))]
    public class PlayerTrigger : MonoBehaviour
    {
        protected Collider Collider => GetComponent<Collider>();

        public UnityEvent OnPlayerEnter;
        public UnityEvent OnPlayerExit;

        protected virtual void OnValidate()
        {
            if (!Collider.isTrigger)
            {
                Debug.LogWarning("Attached collider wasn't set as trigger. Setting as trigger...");
                Collider.isTrigger = true;
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player != null)
                OnPlayerEnter.Invoke();
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player != null)
                OnPlayerExit.Invoke();
        }
    }
}
