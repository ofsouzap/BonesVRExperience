using System.Collections;
using UnityEngine;

namespace BonesVr.Ui
{
    public class FloatHintTarget : MonoBehaviour
    {
        [Tooltip("The game object that is being used for the visualisation of the target but not the interactions")]
        [SerializeField] private GameObject _visualModel;
        public GameObject VisualModel => _visualModel;

        private Vector3 m_VisualModelInitialScales;

        [Header("Shrinking")]

        [Tooltip("The local scale to shrink down to")]
        [SerializeField] private float _shrunkScale = 0f;
        private Vector3 ShrunkScale => _shrunkScale * Vector3.one;

        [Min(0)]
        [Tooltip("How long the shrinking should last")]
        [SerializeField] private float _shrinkDuration;
        protected float ShrinkDuration => _shrinkDuration;

        private void Awake()
        {
            m_VisualModelInitialScales = VisualModel.transform.localScale;
        }

        private Coroutine ShrinkCoroutine { get; set; } = null;

        public void BeginShrinkTarget()
        {
            if (ShrinkCoroutine != null)
                StopCoroutine(ShrinkCoroutine);

            ShrinkCoroutine = StartCoroutine(TargetShrinkCoroutine_Run(ShrunkScale, ShrinkDuration));
        }

        public void BeginGrowTarget()
        {
            if (ShrinkCoroutine != null)
                StopCoroutine(ShrinkCoroutine);

            ShrinkCoroutine = StartCoroutine(TargetShrinkCoroutine_Run(m_VisualModelInitialScales, ShrinkDuration));
        }

        private IEnumerator TargetShrinkCoroutine_Run(Vector3 endScales, float duration)
        {
            Vector3 startScales = VisualModel.transform.localScale;

            float startTime = Time.time;

            while (true)
            {
                float t = (Time.time - startTime) / duration;
                if (t > 1)
                    break;
                else
                    VisualModel.transform.localScale = Vector3.Lerp(startScales, endScales, t);

                yield return new WaitForEndOfFrame();
            }

            VisualModel.transform.localScale = endScales;
        }
    }
}
