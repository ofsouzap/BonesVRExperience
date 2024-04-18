using BonesVr.Characters.Hands;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace BonesVr.Characters.Npcs
{
    [RequireComponent(typeof(Npc))]
    [Obsolete]
    public class AnimatedNpcController : MonoBehaviour
    {
        //[Serializable]
        //public class HandParams
        //{
        //    public Vector3 m_Position = Vector3.zero;
        //    public Quaternion m_Rotation = Quaternion.identity;

        //    public bool m_ThumbTouched = false;
        //    public bool m_IndexTouched = false;
        //    public bool m_GripTouched = false;
        //    [Range(0, 1)] public float m_ThumbVal = 0f;
        //    [Range(0, 1)] public float m_IndexVal = 0f;
        //    [Range(0, 1)] public float m_GripVal = 0f;

        //    public void SetHandParams(Hand hand)
        //    {
        //        hand.transform.SetPositionAndRotation(m_Position, m_Rotation);
        //        hand.SetThumbTouched(m_ThumbTouched);
        //        hand.SetIndexTouched(m_IndexTouched);
        //        hand.SetGripTouched(m_GripTouched);
        //        hand.SetThumbVal(m_ThumbVal);
        //        hand.SetIndexVal(m_IndexVal);
        //        hand.SetGripVal(m_GripVal);
        //    }
        //}

        //[Serializable]
        //public class HeadParams
        //{
        //    public Vector3 m_Position = Vector3.zero;
        //    public Quaternion m_Rotation = Quaternion.identity;

        //    public void SetHeadParams(GameObject head)
        //    {
        //        head.transform.SetPositionAndRotation(m_Position, m_Rotation);
        //    }
        //}

        protected Npc SelfNpc { get; private set; }

        [SerializeField] private Vector3 m_RHPosition = Vector3.zero;
        [SerializeField] private Quaternion m_RHRotation = Quaternion.identity;

        [SerializeField] private bool m_RHThumbTouched = false;
        [SerializeField] private bool m_RHIndexTouched = false;
        [SerializeField] private bool m_RHGripTouched = false;
        [SerializeField][Range(0, 1)] private float m_RHThumbVal = 0f;
        [SerializeField][Range(0, 1)] private float m_RHIndexVal = 0f;
        [SerializeField][Range(0, 1)] private float m_RHGripVal = 0f;

        [SerializeField] private Vector3 m_LHPosition = Vector3.zero;
        [SerializeField] private Quaternion m_LHRotation = Quaternion.identity;

        [SerializeField] private bool m_LHThumbTouched = false;
        [SerializeField] private bool m_LHIndexTouched = false;
        [SerializeField] private bool m_LHGripTouched = false;
        [SerializeField][Range(0, 1)] private float m_LHThumbVal = 0f;
        [SerializeField][Range(0, 1)] private float m_LHIndexVal = 0f;
        [SerializeField][Range(0, 1)] private float m_LHGripVal = 0f;

        [SerializeField] private Vector3 m_HeadPosition = Vector3.zero;
        [SerializeField] private Quaternion m_HeadRotation = Quaternion.identity;

        protected virtual void Awake()
        {
            Debug.LogError("Delete me!!!");
            SelfNpc = GetComponent<Npc>();

            m_RHPosition = SelfNpc.RightHand.transform.position;
            m_RHRotation = SelfNpc.RightHand.transform.rotation;

            m_LHPosition = SelfNpc.LeftHand.transform.position;
            m_LHRotation = SelfNpc.LeftHand.transform.rotation;

            m_HeadPosition = SelfNpc.Head.transform.position;
            m_HeadRotation = SelfNpc.Head.transform.rotation;
        }

        protected virtual void Update()
        {
            SetHandParams(SelfNpc.RightHand, m_RHPosition, m_RHRotation, m_RHThumbTouched, m_RHIndexTouched, m_RHGripTouched, m_RHThumbVal, m_RHIndexVal, m_RHGripVal);
            SetHandParams(SelfNpc.LeftHand, m_LHPosition, m_LHRotation, m_LHThumbTouched, m_LHIndexTouched, m_LHGripTouched, m_LHThumbVal, m_LHIndexVal, m_LHGripVal);
        }

        protected void SetHandParams(Hand hand,
            Vector3 position,
            Quaternion rotation,
            bool thumbTouched,
            bool indexTouched,
            bool gripTouched,
            float thumbVal,
            float indexVal,
            float gripVal)
        {
            hand.transform.SetPositionAndRotation(position, rotation);
            hand.SetThumbTouched(thumbTouched);
            hand.SetIndexTouched(indexTouched);
            hand.SetGripTouched(gripTouched);
            hand.SetThumbVal(thumbVal);
            hand.SetIndexVal(indexVal);
            hand.SetGripVal(gripVal);
        }
    }
}
