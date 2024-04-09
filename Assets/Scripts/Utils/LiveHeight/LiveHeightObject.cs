using UnityEngine;

namespace BonesVr.Utils.LiveHeight
{
    public class LiveHeightObject : LiveHeight
    {
        [Tooltip("The minimum Y position to be at before looking to respawn")]
        [SerializeField] private float _liveMinHeight = 0f;

        protected LiveHeightGroup FindParentLiveHeightGroup()
            => GetComponentInParent<LiveHeightGroup>();

        public bool UsesGrouping
            => FindParentLiveHeightGroup() != null;

        protected override float LiveMinHeight
        {
            get
            {
                LiveHeightGroup group = FindParentLiveHeightGroup();
                if (group != null)
                    return group.GetGroupLiveMinHeight();
                else
                    return _liveMinHeight;
            }
        }
    }
}
