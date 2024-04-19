using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BonesVr.Characters.Npcs.Animation
{
    public class NpcAnimationRecorder : MonoBehaviour
    {
        [Tooltip("The folder to store the output clips in (relative to the Assets folder)")]
        [SerializeField] private string _outputPath = "";

        [Tooltip("The name of the asset to create for the clip")]
        public string m_OutputAssetName = "Clip";

        protected string GetOutputPath()
        {
            string path = $"Assets/{_outputPath}/{m_OutputAssetName}.asset";

            if (AssetDatabase.LoadAssetAtPath<Object>(path) == null)
                return path;
            else
            {
                for (int i = 0; ; i++)
                {
                    path = $"Assets/{_outputPath}/{m_OutputAssetName}.{i}.asset";
                    if (AssetDatabase.LoadAssetAtPath<Object>(path) == null)
                        return path;
                }
            }
        }

        [Tooltip("How long to delay between storing a keyframe for the animation")]
        [SerializeField] private float _captureDelay = .2f;
        protected float CaptureDelay => _captureDelay;

        [SerializeField] private InputActionReference _textBoxKeyframeAction;
        protected InputAction TextBoxKeyframeAction => _textBoxKeyframeAction != null ? _textBoxKeyframeAction.action : null;

        [SerializeField] private NpcAnimationRecordingTargets _recordingTargets;
        protected NpcAnimationRecordingTargets RecordingTargets => _recordingTargets;

        /// <summary>
        /// This is null if and only if the recorder is not currently recording.
        /// </summary>
        private NpcAnimationClipBuilder m_AnimationBuilder;
        private float m_CaptureStartTime;
        private float m_NextCaptureTime;

        public bool IsRecording => m_AnimationBuilder != null;

        protected virtual void Awake()
        {
            m_AnimationBuilder = null;
        }

        protected virtual void Update()
        {
            if (IsRecording)
            {
                if (Time.time >= m_NextCaptureTime)
                {
                    float t = Time.time - m_CaptureStartTime;
                    var snap = RecordingTargets.CreateSnapshot();

                    m_AnimationBuilder.AddKeyframe(t, snap);

                    m_NextCaptureTime = Time.time + CaptureDelay;
                }
            }
        }

        protected virtual void OnEnable()
        {
            if (TextBoxKeyframeAction != null)
                TextBoxKeyframeAction.started += TextBoxKeyframeActionPressed;
        }

        protected virtual void OnDisable()
        {
            if (TextBoxKeyframeAction != null)
                TextBoxKeyframeAction.started -= TextBoxKeyframeActionPressed;

            if (IsRecording)
                StopRecording();
        }

        public void StartRecording()
        {
            m_AnimationBuilder = new();
            m_CaptureStartTime = Time.time;
            m_NextCaptureTime = Time.time;
        }

        public void StopRecording()
        {
            NpcAnimationClip clip = m_AnimationBuilder.Build();
            string assetPath = GetOutputPath();

            AssetDatabase.CreateAsset(clip, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            m_AnimationBuilder = null;

            Debug.Log($"Saved output at {assetPath}");
        }

        private void TextBoxKeyframeActionPressed(InputAction.CallbackContext _)
        {
            if (IsRecording)
                AddTextBoxKeyframe();
        }

        public void AddTextBoxKeyframe()
        {
            if (IsRecording)
            {
                float t = Time.time - m_CaptureStartTime;
                m_AnimationBuilder.AddTextBoxKeyframe(t);
            }
            else
                Debug.LogWarning("Trying to add text box keyframe when not recording");
        }
    }
}
