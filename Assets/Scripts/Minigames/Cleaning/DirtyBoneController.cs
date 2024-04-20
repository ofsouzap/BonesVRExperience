using BonesVr.Utils;
using System.Linq;
using UnityEngine;

namespace BonesVr.Minigames.Cleaning
{
    public class DirtyBoneController : MonoBehaviour
    {
        private const string k_MaterialDirtinessRef = "_Dirtiness";

        [SerializeField] private MeshRenderer _meshRenderer;
        public MeshRenderer MeshRenderer => _meshRenderer;

        public MeshFilter MeshFilter => MeshRenderer.gameObject.GetComponent<MeshFilter>();

        [Range(0, 1)]
        [SerializeField] private float _initialDirtiness;
        private float InitialDirtiness => _initialDirtiness;

        private float m_Dirtiness;

        protected virtual void Awake()
        {
            if (MeshRenderer == null)
                _meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (MeshRenderer == null)
                Debug.LogError("No renderer component set or found");
        }

        protected virtual void Start()
        {
            SetDirtiness(InitialDirtiness);
        }

        protected virtual void OnValidate()
        {
            if (_meshRenderer != null && _meshRenderer.gameObject.GetComponent<MeshFilter>() == null)
                Debug.LogError("Mesh renderer doesn't have a mesh filter attached");
        }

        protected void UpdateMaterialDirtiness()
        {
            MeshRenderer.material.SetFloat(k_MaterialDirtinessRef, m_Dirtiness);
        }

        public float GetDirtiness() => m_Dirtiness;

        /// <summary>
        /// Checks if the bone is fully clean by checking its dirtiness value is approximately 0.
        /// </summary>
        public bool IsFullyClean => GetDirtiness() < 1e-6f;

        public void SetDirtiness(float val)
        {
            m_Dirtiness = Mathf.Clamp01(val);
            UpdateMaterialDirtiness();
        }

        public void ChangeDirtiness(float delta)
        {
            m_Dirtiness = Mathf.Clamp01(m_Dirtiness + delta);
            UpdateMaterialDirtiness();
        }
    }
}
