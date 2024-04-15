using UnityEngine;

namespace BonesVr.Minigames.Cleaning
{
    public class DirtyBoneShaderController : MonoBehaviour
    {
        [SerializeField] private int _dirtinessTextureSize = 512;
        public int DirtinessTextureSize => _dirtinessTextureSize;

        private readonly int _dirtinessTextureSizeSqr;
        public int DirtinessTextureSizeSqr => _dirtinessTextureSizeSqr;

        [SerializeField] private Renderer _renderer;
        protected Renderer Renderer => _renderer;

        private Texture2D m_DirtinessTexture;

        public DirtyBoneShaderController() : base()
        {
            _dirtinessTextureSizeSqr = _dirtinessTextureSize * _dirtinessTextureSize;
        }

        protected static Color32 DirtinessValueToTexCol(float dirtinessValue)
        {
            byte v = (byte)Mathf.RoundToInt(255 * dirtinessValue);
            return new(v, v, v, 255);
        }

        protected virtual void Awake()
        {
            if (Renderer == null)
                _renderer = GetComponentInChildren<Renderer>();
            if (Renderer == null)
                Debug.LogError("No renderer component set or found");

            m_DirtinessTexture = new(DirtinessTextureSize, DirtinessTextureSize, TextureFormat.RGB24, false);
            DrawFill(1f);
        }

        protected virtual void Start()
        {
            Renderer.material.SetTexture("_DirtinessTexture", m_DirtinessTexture);
        }

        public void DrawCircle(float dirtiness, float centerU, float centerV, float radius, bool applyChanges = true)
        {
            Color32 col = DirtinessValueToTexCol(dirtiness);

            int centerX = Mathf.FloorToInt(DirtinessTextureSize * centerU);
            int centerY = Mathf.FloorToInt(DirtinessTextureSize * centerV);

            int radiusPix = Mathf.FloorToInt(DirtinessTextureSize * radius);
            int radiusPixSqr = radiusPix * radiusPix;

            for (int dy = -radiusPix; dy <= radiusPix; dy++)
            {
                int dySqr = dy * dy;
                int xRange = Mathf.RoundToInt(Mathf.Sqrt(radiusPixSqr - dySqr));

                Color32[] arr = new Color32[(2 * xRange) + 1];
                for (int i = 0; i < arr.Length; i++) arr[i] = col;

                m_DirtinessTexture.SetPixels32(
                    centerX - xRange,
                    centerY + dy,
                    (2 * xRange) + 1,
                    1,
                    arr
                );
            }

            if (applyChanges)
                m_DirtinessTexture.Apply();
        }

        public void DrawFill(float dirtiness, bool applyChanges = true)
        {
            Color32 col = DirtinessValueToTexCol(dirtiness);

            Color32[] arr = new Color32[DirtinessTextureSizeSqr];
            for (int i = 0; i < arr.Length; i++) arr[i] = col;

            m_DirtinessTexture.SetPixels32(arr);

            if (applyChanges)
                m_DirtinessTexture.Apply();
        }

        public void DrawApply()
        {
            m_DirtinessTexture.Apply();
        }
    }
}
