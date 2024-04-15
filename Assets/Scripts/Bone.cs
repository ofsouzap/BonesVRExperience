using UnityEngine;

namespace BonesVr
{
    public enum BoneType
    {
        // NOTE: When editing, so that the serialized fields in the Unity game objects aren't broken, don't ever reuse the enum indexes.
        //       E.g. I added LeftRadius after RightHumerus so it has a greater enum index than it but I declare it first for neatness reasons.
        Unknown = 0, // This is only really so that I have a default option. It shouldn't every really be used
        Skull = 1,
        Mandible = 2,
        LeftHumerus = 3,
        LeftRadius = 8,
        LeftUlna = 9,
        RightHumerus = 4,
        RightRadius = 10,
        RightUlna = 11,
        VertebraUpper = 5,
        VertebraMid = 6,
        VertebraLower = 7,
        LeftFemur = 12,
        RightFemur = 13,
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
