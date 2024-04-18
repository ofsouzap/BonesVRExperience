using BonesVr.Characters.Hands;
using UnityEngine;

namespace BonesVr.Development.AnimationRecording
{
    public class PlayerAnimatedHandMimicController : MonoBehaviour
    {
        public AnimatedHandController target;
        public PlayerHandController playerHand;

        protected virtual void Update()
        {
            //target.m_Position = playerHand.transform.position;
            //target.m_Rotation = playerHand.transform.rotation;
            target.m_ThumbTouched = playerHand.ThumbTouched;
            target.m_IndexTouched = playerHand.TriggerTouched;
            target.m_GripTouched = playerHand.GripTouched;
            target.m_ThumbVal = playerHand.ThumbVal;
            target.m_IndexVal = playerHand.TriggerVal;
            target.m_GripVal = playerHand.GripVal;
        }
    }
}
