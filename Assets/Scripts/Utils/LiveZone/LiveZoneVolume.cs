using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BonesVr.Utils.LiveZone
{
    [RequireComponent(typeof(Collider))]
    public class LiveZoneVolume : MonoBehaviour
    {
        private ICollection<Collider> m_Colliders;

        protected virtual void Awake()
        {
            m_Colliders = GetComponents<Collider>()
                .Where(c => c.isTrigger)
                .ToArray();

            if (m_Colliders.Count == 0)
                Debug.LogWarning("No valid trigger colliders found");
        }

        protected virtual void OnValidate()
        {
            if (HasLiveZoneUserComponentOrInDescendant(transform))
                Debug.LogError("Live zone volume can't have a live zone user component in its game object or in its descendants' game objects");
        }

        private static bool HasLiveZoneUserComponentOrInDescendant(Transform t)
        {
            if (t.TryGetComponent<LiveZoneUser>(out var _))
                return true;
            else
            {
                foreach (Transform child in t)
                    if (HasLiveZoneUserComponentOrInDescendant(child))
                        return true;

                return false;
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            LiveZoneUser zoneUser = other.GetComponentInParent<LiveZoneUser>();
            if (zoneUser != null)
                zoneUser.ExitedLiveZoneVolume(this);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            LiveZoneUser zoneUser = other.GetComponentInParent<LiveZoneUser>();
            if (zoneUser != null)
                zoneUser.EnteredLiveZoneVolume(this);
        }
    }
}
