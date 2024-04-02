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
            Vector3[] poss = CalcLinePositions(transform.position, Target.transform.position, Camera.main.transform.position);
            LineRenderer.SetPositions(poss);
            LineRenderer.positionCount = poss.Length;
        }

        protected static Vector3[] CalcLinePositions(Vector3 start, Vector3 end, Vector3 camera)
        {
            Vector3 displacement = end - start;

            Vector3 cameraEndDisplacement = end - camera;

            Vector2 planeStart = new(start.x, start.z);

            Vector2 planeForward = new Vector2(
                cameraEndDisplacement.x,
                cameraEndDisplacement.z
            ).normalized;
            Vector3 planeForwardV3 = new(planeForward.x, 0, planeForward.y);

            Vector3 planeRightV3 = Vector3.Cross(planeForwardV3, Vector3.up);
            Vector2 planeRight = new Vector2(planeRightV3.x, planeRightV3.z).normalized;

            float distForward = Vector3.Dot(displacement, planeForward);
            float distRight = Vector3.Dot(displacement, planeRight);

            Vector2 midpos1 = planeStart + (distRight * 0.1f * planeRight);
            Vector2 midpos2 = planeStart + ((distForward * planeForward) + (distRight * 0.9f * planeRight));

            Vector3[] poss = new Vector3[]
            {
                start,
                new(midpos1.x, start.y, midpos1.y),
                new(midpos2.x, end.y, midpos2.y),
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

