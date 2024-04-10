using UnityEngine;

namespace BonesVr
{
    public enum BoneType
    {
        Unknown, // This is only really so that I have a default option. It shouldn't every really be used
        Skull,
        Mandible,
        LeftHumerus,
        RightHumerus,
        VertebraUpper,
        VertebraMid,
        VertebraLower,
    }

    public class Bone : MonoBehaviour
    {
        [SerializeField] private BoneType _boneType = BoneType.Unknown;
        public BoneType Type => _boneType;

        protected virtual void OnValidate()
        {
            if (Type == BoneType.Unknown)
                Debug.LogWarning("No bone type set");
        }
    }
}
