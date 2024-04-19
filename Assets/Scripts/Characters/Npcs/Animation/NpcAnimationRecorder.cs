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

        [SerializeField] private InputActionReference _startRecordingAction;
        protected InputAction StartRecordingAction => _startRecordingAction != null ? _startRecordingAction.action : null;

        [SerializeField] private InputActionReference _stopRecordingAction;
        protected InputAction StopRecordingAction => _stopRecordingAction != null ? _stopRecordingAction.action : null;

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
            if (StartRecordingAction != null) StartRecordingAction.performed += StartRecordingActionPressed;
            if (StopRecordingAction != null) StopRecordingAction.performed += StopRecordingActionPressed;
            if (TextBoxKeyframeAction != null) TextBoxKeyframeAction.performed += TextBoxKeyframeActionPressed;
        }

        protected virtual void OnDisable()
        {
            if (StartRecordingAction != null) StartRecordingAction.performed -= StartRecordingActionPressed;
            if (StopRecordingAction != null) StopRecordingAction.performed -= StopRecordingActionPressed;
            if (TextBoxKeyframeAction != null) TextBoxKeyframeAction.performed -= TextBoxKeyframeActionPressed;

            if (IsRecording)
                StopRecording();
        }

        private void StartRecordingActionPressed(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                if (!IsRecording)
                    StartRecording();
            }
        }

        public void StartRecording()
        {
            m_AnimationBuilder = new();
            m_CaptureStartTime = Time.time;
            m_NextCaptureTime = Time.time;

            Debug.Log("Started recording...");
        }

        private void StopRecordingActionPressed(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                if (IsRecording)
                    StopRecording();
            }
        }

        public void StopRecording()
        {
            if (IsRecording)
            {
                NpcAnimationClip clip = m_AnimationBuilder.Build();
                string assetPath = GetOutputPath();

                AssetDatabase.CreateAsset(clip, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                m_AnimationBuilder = null;

                Debug.Log($"Saved output at {assetPath}");
            }
        }

        private void TextBoxKeyframeActionPressed(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                if (IsRecording)
                    AddTextBoxKeyframe();
            }
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
