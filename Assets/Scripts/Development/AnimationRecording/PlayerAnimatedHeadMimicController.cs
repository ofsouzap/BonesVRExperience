using BonesVr.Characters.Npcs;
using System;
using UnityEngine;

namespace BonesVr.Development.AnimationRecording
{
    [Obsolete]
    public class PlayerAnimatedHeadMimicController : MonoBehaviour
    {
        public AnimatedNpcHeadController target;
        public Transform playerHead;

        protected virtual void Update()
        {
            target.m_Position = playerHead.transform.position;
            target.m_Rotation = playerHead.transform.rotation;
        }
    }
}
