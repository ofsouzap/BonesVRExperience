using BonesVr.Ui;
using TMPro;
using UnityEngine;

namespace BonesVr.UI
{
    public class FloatHint : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        protected LineRenderer LineRenderer => _lineRenderer;

        [SerializeField] private Canvas _uiCanvas;
        public Canvas UiCanvas => _uiCanvas;

        private FloatHintTarget _target;
        public FloatHintTarget Target => _target;

        [SerializeField] private TMP_Text _text;
        protected TMP_Text Text => _text;

        [Tooltip("The message to display in the hint box")]
        [SerializeField] private string _message;
        public string Message => _message;

        protected Vector2 TextRelativePos { get; private set; }

        public bool Shown { get; private set; }

        protected virtual void Awake()
        {
            if (Text == null) Debug.LogWarning("No text component provided");
            else Text.text = Message;

            _target = GetComponentInChildren<FloatHintTarget>();
            if (Target == null) Debug.LogWarning("No float hint target component found");

            if (LineRenderer == null) Debug.LogWarning("No LineRenderer component provided");
            if (UiCanvas == null) Debug.LogWarning("No UI Canvas component provided");

            HideHint();
        }

        protected virtual void Start()
        {
            TextRelativePos = new(
                UiCanvas.transform.localPosition.x - Target.transform.localPosition.x,
                UiCanvas.transform.localPosition.y - Target.transform.localPosition.y
            );
        }

        protected virtual void Update()
        {
            RefreshTextTransform();
            RefreshLinePositions();
        }

        protected virtual void OnValidate()
        {
            // Update text in text box as message is edited in this component's Inspector
            if (Text != null && Message != null)
                Text.text = Message;
        }

        private void RefreshTextTransform()
        {
            Transform camera = Camera.main.transform;
            Vector3 cameraTargetDisp = Target.transform.position - camera.position;
            Vector3 targetViewPlaneRight = Vector3.Cross(cameraTargetDisp, Vector3.up).normalized;
            Vector3 targetViewPlaneUp = Vector3.Cross(targetViewPlaneRight, cameraTargetDisp).normalized;

            UiCanvas.transform.SetPositionAndRotation(
                Target.transform.position - (TextRelativePos.x * targetViewPlaneRight) + (TextRelativePos.y * targetViewPlaneUp),
                Quaternion.LookRotation(UiCanvas.transform.position - camera.position)
            );
        }

        private void RefreshLinePositions()
        {
            Vector3[] poss = CalcLinePositions(Target.transform.position, UiCanvas.transform.position, Camera.main.transform.position);
            LineRenderer.SetPositions(poss);
            LineRenderer.positionCount = poss.Length;
        }

        protected static Vector3[] CalcLinePositions(Vector3 start, Vector3 end, Vector3 camera)
        {
            const float fac1 = 0.25f;
            const float fac2 = 0.75f;

            Vector3 displacement = end - start;

            Vector3 billboardForward = (end - camera).normalized;
            Vector3 billboardRight = Vector3.Cross(billboardForward, Vector3.up);
            Vector3 billboardUp = Vector3.Cross(billboardRight, billboardForward);

            float distForward = Vector3.Dot(displacement, billboardForward);
            float distRight = Vector3.Dot(displacement, billboardRight);
            float distUp = Vector3.Dot(displacement, billboardUp);

            Vector3 midpos1 = start + (distForward * billboardForward) + (fac1 * distRight * billboardRight);
            Vector3 midpos2 = start + (distForward * billboardForward) + (fac2 * distRight * billboardRight) + (distUp * billboardUp);

            Vector3[] poss = new Vector3[]
            {
                start,
                midpos1,
                midpos2,
                end,
            };

            return poss;
        }

        private void ShowLine() => LineRenderer.enabled = true;
        private void HideLine() => LineRenderer.enabled = false;

        private void ShowUiText() => UiCanvas.enabled = true;
        private void HideUiText() => UiCanvas.enabled = false;

        public void ShowHint()
        {
            ShowLine();
            ShowUiText();
            Target.BeginShrinkTarget();
            Shown = true;
        }

        public void HideHint()
        {
            HideLine();
            HideUiText();
            Target.BeginGrowTarget();
            Shown = false;
        }
    }
}
