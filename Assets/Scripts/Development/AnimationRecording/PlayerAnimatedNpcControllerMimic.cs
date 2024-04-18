using BonesVr.Characters.Hands;
using BonesVr.Characters.Npcs;
using System;
using UnityEngine;

namespace BonesVr.Development.AnimationRecording
{
    [Obsolete]
    public class PlayerAnimatedNpcControllerMimic : AnimatedNpcController
    {
        [SerializeField] private PlayerHandController _rightHand;
        protected PlayerHandController RightHand => _rightHand;

        [SerializeField] private PlayerHandController _leftHand;
        protected PlayerHandController LeftHand => _leftHand;

        [SerializeField] private GameObject _head;
        protected GameObject Head => _head;

        protected override void Awake()
        {
            Debug.LogError("Delete me!!!");
            // Don't call `base.Awake()` as it sets the param values from the Npc component but I don't want to use the Npc component in this override

            GetComponent<Npc>().enabled = false;

            if (RightHand == null) Debug.LogError("No right hand set");
            if (LeftHand == null) Debug.LogError("No left hand set");
            if (Head == null) Debug.LogError("No head set");
        }

        protected override void Update()
        {
            // Don't call `base.Update()` as this would try to set the param values on the NPC instance

            UpdateParamsFromSelf();
        }

        protected void UpdateParamsFromSelf()
        {
            //m_RHPosition = RightHand.transform.position;
            //m_RHRotation = RightHand.transform.rotation;

            //m_RHThumbTouched = RightHand.ThumbTouched;
            //m_RHIndexTouched = RightHand.TriggerTouched;
            //m_RHGripTouched = RightHand.GripTouched;
            //m_RHThumbVal = RightHand.ThumbVal;
            //m_RHIndexVal = RightHand.TriggerVal;
            //m_RHGripVal = RightHand.GripVal;

            // TODO - LH
            // TODO - head
        }

        //protected static void UpdateHandParamsFromPlayerHand(HandParams handParams, PlayerHandController playerHand)
        //{
        //    handParams.m_Position = playerHand.transform.position;
        //    handParams.m_Rotation = playerHand.transform.rotation;

        //    handParams.m_ThumbTouched = playerHand.ThumbTouched;
        //    handParams.m_IndexTouched = playerHand.TriggerTouched;
        //    handParams.m_GripTouched = playerHand.GripTouched;

        //    handParams.m_ThumbVal = playerHand.ThumbVal;
        //    handParams.m_IndexVal = playerHand.TriggerVal;
        //    handParams.m_GripVal = playerHand.GripVal;
        //}

        //protected static void UpdateHeadParamsFromPlayerHead(HeadParams headParams, GameObject playerHead)
        //{
        //    headParams.m_Position = playerHead.transform.position;
        //    headParams.m_Rotation = playerHead.transform.rotation;
        //}

        //protected void UpdateParamsFromSelf()
        //{
        //    UpdateHandParamsFromPlayerHand(GetRightHandParams(), RightHand);
        //    UpdateHandParamsFromPlayerHand(GetLeftHandParams(), LeftHand);
        //    UpdateHeadParamsFromPlayerHead(GetHeadParams(), Head);
        //}
    }
}
