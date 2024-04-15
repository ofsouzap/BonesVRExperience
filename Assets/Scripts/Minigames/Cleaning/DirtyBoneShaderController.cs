using UnityEngine;

namespace BonesVr.Minigames.Cleaning
{
    public class DirtyBoneShaderController : MonoBehaviour
    {
        [SerializeField] private int _dirtinessTextureSize = 512;
        public int DirtinessTextureSize => _dirtinessTextureSize;

        private readonly int _dirtinessTextureSizeSqr;
        public int DirtinessTextureSizeSqr => _dirtinessTextureSizeSqr;

        [SerializeField] private MeshRenderer _meshRenderer;
        public MeshRenderer MeshRenderer => _meshRenderer;

        public MeshFilter MeshFilter => MeshRenderer.gameObject.GetComponent<MeshFilter>();

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
            if (MeshRenderer == null)
                _meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (MeshRenderer == null)
                Debug.LogError("No renderer component set or found");

            m_DirtinessTexture = new(DirtinessTextureSize, DirtinessTextureSize, TextureFormat.RGB24, false);
            DrawFill(1f);
        }

        protected virtual void Start()
        {
            MeshRenderer.material.SetTexture("_DirtinessTexture", m_DirtinessTexture);
        }

        protected virtual void OnValidate()
        {
            if (_meshRenderer != null && _meshRenderer.gameObject.GetComponent<MeshFilter>() == null)
                Debug.LogError("Mesh renderer doesn't have a mesh filter attached");
        }

        public void DrawCircle(float dirtiness, float centerU, float centerV, float radius, bool applyChanges = true)
        {
            Color32 col = DirtinessValueToTexCol(dirtiness);

            int centerX = Mathf.FloorToInt(DirtinessTextureSize * centerU);
            int centerY = Mathf.FloorToInt(DirtinessTextureSize * centerV);
            int radiusPix = Mathf.FloorToInt(DirtinessTextureSize * radius);

            int radiusPixSqr = radiusPix * radiusPix;

            int yMin = Mathf.Clamp(centerY - radiusPix, 0, DirtinessTextureSize - 1);
            int yMax = Mathf.Clamp(centerY + radiusPix, 0, DirtinessTextureSize - 1);

            for (int y = yMin + 1; y < yMax; y++) // (Don't include endpoints as they result in drawing 0 pixels)
            {
                int dy = y - centerY;
                int dySqr = dy * dy;

                int xHalfRangeMaximum = Mathf.RoundToInt(Mathf.Sqrt(radiusPixSqr - dySqr));

                int xMin = Mathf.Clamp(centerX - xHalfRangeMaximum, 0, DirtinessTextureSize - 1);
                int xMax = Mathf.Clamp(centerX + xHalfRangeMaximum, 0, DirtinessTextureSize - 1);

                int xRangeTrue = xMax - xMin;

                Color32[] arr = new Color32[xRangeTrue];
                for (int i = 0; i < arr.Length; i++) arr[i] = col;

                m_DirtinessTexture.SetPixels32(
                    xMin,
                    y,
                    xRangeTrue,
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
