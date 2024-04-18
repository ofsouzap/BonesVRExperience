using UnityEngine;

namespace BonesVr.Characters.Hands
{
    public class AnimatedHandController : MonoBehaviour
    {
        [SerializeField] private bool m_AllowNoHandSet = false; // Specifically for the recording setup where an instance of this class is used without an attached Hand

        [SerializeField] private Hand _hand;
        protected Hand Hand => _hand;

        //public Vector3 m_Position;
        //public Quaternion m_Rotation;
        public bool m_ThumbTouched = false;
        public bool m_IndexTouched = false;
        public bool m_GripTouched = false;
        [Range(0, 1)] public float m_ThumbVal = 0f;
        [Range(0, 1)] public float m_IndexVal = 0f;
        [Range(0, 1)] public float m_GripVal = 0f;

        protected virtual void Awake()
        {
            if (!m_AllowNoHandSet && Hand == null)
                Debug.LogError("No hand component set");
        }

        protected virtual void Update()
        {
            if (Hand != null)
            {
                //Hand.transform.SetPositionAndRotation(m_Position, m_Rotation);
                Hand.SetThumbTouched(m_ThumbTouched);
                Hand.SetIndexTouched(m_IndexTouched);
                Hand.SetGripTouched(m_GripTouched);
                Hand.SetThumbVal(m_ThumbVal);
                Hand.SetIndexVal(m_IndexVal);
                Hand.SetGripVal(m_GripVal);
            }
        }
    }
}
