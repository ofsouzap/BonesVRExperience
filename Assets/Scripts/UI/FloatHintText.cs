using UnityEngine;
using UnityEngine.UI;

namespace BonesVr.Ui
{
    public class FloatHintText : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        protected LineRenderer LineRenderer { get; private set; }

        [SerializeField] private Canvas _uiCanvas;
        protected Canvas UiCanvas { get; private set; }

        [SerializeField] private FloatHintTarget _target;
        protected FloatHintTarget Target { get; private set; }

        public bool Shown { get; private set; }

        private void Awake()
        {
            LineRenderer = _lineRenderer;
            if (LineRenderer == null)
                LineRenderer = GetComponentInChildren<LineRenderer>();
            if (LineRenderer == null)
                Debug.LogWarning("No LineRenderer component found");

            UiCanvas = _uiCanvas;
            if (UiCanvas == null)
                UiCanvas = GetComponentInChildren<Canvas>();
            if (UiCanvas == null)
                Debug.LogWarning("No UI Canvas component found");

            Target = _target;
            if (Target == null)
                Target = GetComponentInChildren<FloatHintTarget>();
            if (Target == null)
                Debug.LogWarning("No target found");

            //HideHint();
            ShowHint();
        }

        protected virtual void Update()
        {
            RefreshTextTransform();
            RefreshLinePositions();
        }

        private void RefreshTextTransform()
        {
            Transform camera = Camera.main.transform;

            // Face towards camera
            UiCanvas.transform.rotation = Quaternion.LookRotation(UiCanvas.transform.position - camera.position);

            // TODO - move hint text so that it is a good distance from camera and also in an appropriate position not too far away from target
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
            Shown = true;
        }
        
        public void HideHint()
        {
            HideLine();
            HideUiText();
            Shown = false;
        }
    }
}

