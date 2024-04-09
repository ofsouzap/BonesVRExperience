using System.Linq;
using UnityEngine;

namespace BonesVr.Utils.LiveHeight
{
    public class LiveHeightGroup : MonoBehaviour
    {
        private static readonly Color c_UserGizmoColor = new(1f, .3f, .3f, .7f);
        private const float c_UserGizmoRadius = .02f;

        [Tooltip("The minimum Y position to be at before looking to respawn")]
        [SerializeField] private float _groupLiveMinHeight = 0f;

        public float GetGroupLiveMinHeight() => _groupLiveMinHeight;

        public LiveHeightObject[] GetGroupUsers() => GetComponentsInChildren<LiveHeightObject>();

        protected virtual void OnValidate()
        {
            if (GetComponentsInParent<LiveHeightGroup>()
                .Where(x => x != this)
                .Any())
                Debug.LogError("Live height groups cannot be nested");
        }

        protected void OnDrawGizmosSelected()
        {
            foreach (var user in GetGroupUsers())
            {
                Gizmos.color = c_UserGizmoColor;
                Gizmos.DrawSphere(user.transform.position, c_UserGizmoRadius);
            }
        }
    }
}
