using UnityEngine;

namespace BonesVr.Characters.Hands
{
    public class NpcAnimatedHandController : MonoBehaviour
    {
        protected Hand Hand { get; private set; }

        public bool ThumbTouched { get => Hand.ThumbTouched; set => Hand.SetThumbTouched(value); }
        public bool IndexTouched { get => Hand.IndexTouched; set => Hand.SetIndexTouched(value); }
        public bool GripTouched { get => Hand.GripTouched; set => Hand.SetGripTouched(value); }
        public float ThumbVal { get => Hand.ThumbVal; set => Hand.SetThumbVal(value); }
        public float IndexVal { get => Hand.IndexVal; set => Hand.SetIndexVal(value); }
        public float GripVal { get => Hand.GripVal; set => Hand.SetGripVal(value); }

        protected virtual void Awake()
        {
            Hand = GetComponent<Hand>();
        }
    }
}
