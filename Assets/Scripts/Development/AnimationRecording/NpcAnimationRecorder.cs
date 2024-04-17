using UnityEditor.Animations;
using UnityEngine;

namespace BonesVr.Development.AnimationRecording
{
    public class NpcAnimationRecorder : MonoBehaviour
    {
        [Tooltip("The root game object of the hierarchy being recorded")]
        [SerializeField] private GameObject _targetRoot;
        protected GameObject TargetRoot => _targetRoot;

        [Tooltip("The animation clip to write the recording to")]
        [SerializeField] private AnimationClip _outputClip;
        protected AnimationClip OutputClip => _outputClip;

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
            m_Recorder.BindComponent(TargetRoot.transform);
        }

        protected virtual void Update()
        {
            if (m_Recording)
            {
                m_Recorder.TakeSnapshot(Time.deltaTime);
            }
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
