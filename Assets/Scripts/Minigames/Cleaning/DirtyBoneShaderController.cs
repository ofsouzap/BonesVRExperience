using System.Linq;
using UnityEngine;

namespace BonesVr.Minigames.Cleaning
{
    public class DirtyBoneShaderController : MonoBehaviour
    {
        [SerializeField] private int _dirtinessTextureSize = 512;
        protected int DirtinessTextureSize => _dirtinessTextureSize;

        [SerializeField] private Renderer _renderer;
        protected Renderer Renderer => _renderer;

        private Texture2D m_DirtinessTexture;

        protected static byte DirtinessValueToTexVal(float dirtinessValue) => (byte)Mathf.RoundToInt(255 * dirtinessValue);

        protected virtual void Awake()
        {
            if (Renderer == null)
                _renderer = GetComponentInChildren<Renderer>();
            if (Renderer == null)
                Debug.LogError("No renderer component set or found");

            m_DirtinessTexture = new(DirtinessTextureSize, DirtinessTextureSize, TextureFormat.RGB24, false);
            FillDirtinessTexture(1f);
        }

        protected virtual void Start()
        {
            Renderer.material.SetTexture("_DirtinessTexture", m_DirtinessTexture);
        }

        protected void FillDirtinessTexture(float dirtiness)
        {
            byte val = DirtinessValueToTexVal(dirtiness);
            Color32 col = new(val, val, val, 255);

            Color32[] arr = new Color32[m_DirtinessTexture.width * m_DirtinessTexture.height];
            for (int i = 0; i < arr.Length; i++) arr[i] = col;

            m_DirtinessTexture.SetPixels32(arr);
            m_DirtinessTexture.Apply();
        }
    }
}
