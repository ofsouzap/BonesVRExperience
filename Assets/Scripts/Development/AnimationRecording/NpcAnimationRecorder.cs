using BonesVr.Characters.Hands;
using BonesVr.Characters.Npcs;
using System;
using UnityEditor.Animations;
using UnityEngine;

namespace BonesVr.Development.AnimationRecording
{
    public class NpcAnimationRecorder : MonoBehaviour
    {
        [Serializable]
        public struct CharacterRecordingTargets
        {
            public Transform m_RootTransform;
            public Transform m_RightHandTransform;
            public AnimatedHandController m_RightHand;
            public Transform m_LeftHandTransform;
            public AnimatedHandController m_LeftHand;
            public Transform m_Head;

            public readonly void BindRecorder(GameObjectRecorder recorder)
            {
                recorder.BindComponent(m_RootTransform);
                recorder.BindComponent(m_RightHandTransform);
                recorder.BindComponent(m_RightHand);
                recorder.BindComponent(m_LeftHandTransform);
                recorder.BindComponent(m_LeftHand);
                recorder.BindComponent(m_Head);
            }
        }

        [Tooltip("The root game object of the hierarchy being recorded")]
        [SerializeField] private GameObject _targetRoot;
        protected GameObject TargetRoot => _targetRoot;

        [Tooltip("The animation clip to write the recording to")]
        [SerializeField] private AnimationClip _outputClip;
        protected AnimationClip OutputClip => _outputClip;

        [Tooltip("The references to the components that need to be recorded for a character animation")]
        [SerializeField] private CharacterRecordingTargets _recordingTargets;
        protected CharacterRecordingTargets RecordingTargets => _recordingTargets;

        private GameObjectRecorder m_Recorder;
        private bool m_Recording;

        public bool IsRecording => m_Recording;

        protected virtual void Awake()
        {
            if (TargetRoot == null)
                Debug.LogError("No target root set");

            if (OutputClip == null)
                Debug.LogError("No output clip set");

            m_Recording = false;

            m_Recorder = new(TargetRoot);
            RecordingTargets.BindRecorder(m_Recorder);
        }

        protected virtual void Update()
        {
            if (m_Recording)
            {
                m_Recorder.TakeSnapshot(Time.deltaTime);
            }
        }

        protected virtual void OnDisable()
        {
            if (m_Recording)
                StopRecording();
        }

        public void StartRecording()
        {
            m_Recording = true;
        }

        public void StopRecording()
        {
            m_Recording = false;

            m_Recorder.SaveToClip(OutputClip);
            m_Recorder.ResetRecording();
        }
    }
}
