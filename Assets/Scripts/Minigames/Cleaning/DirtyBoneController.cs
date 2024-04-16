using BonesVr.Utils;
using System.Linq;
using UnityEngine;

namespace BonesVr.Minigames.Cleaning
{
    public class DirtyBoneController : MonoBehaviour
    {
        [SerializeField] private int _dirtinessTextureSize = 512;
        public int DirtinessTextureSize => _dirtinessTextureSize;

        public int DirtinessTextureSizeSqr => DirtinessTextureSize * DirtinessTextureSize;

        [SerializeField] private MeshRenderer _meshRenderer;
        public MeshRenderer MeshRenderer => _meshRenderer;

        public MeshFilter MeshFilter => MeshRenderer.gameObject.GetComponent<MeshFilter>();

        [Header("Initial Random Dirt")]

        [SerializeField] private int _initialRandomDirtSpotsCount;
        protected int InitialRandomDirtSpotsCount => _initialRandomDirtSpotsCount;

        [Tooltip("The standard deviation of the Gaussian distribution used to set the initial random dirt spots on the dirtiness texture. This value is in normalised UV coordinates")]
        [SerializeField] private float _initialRandomDirtFac;
        protected float InitialRandomDirtFac => _initialRandomDirtFac;

        private float m_BaseDirtiness;
        private Texture2D m_DirtinessTexture;

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
            m_BaseDirtiness = 1f;
        }

        protected virtual void Start()
        {
            MeshRenderer.material.SetTexture("_DirtinessTexture", m_DirtinessTexture);
            UpdateBaseDirtiness();

            DrawRandomDirtSpots(InitialRandomDirtSpotsCount, InitialRandomDirtFac, applyChanges: true);
        }

        protected virtual void OnValidate()
        {
            if (_meshRenderer != null && _meshRenderer.gameObject.GetComponent<MeshFilter>() == null)
                Debug.LogError("Mesh renderer doesn't have a mesh filter attached");
        }

        protected void UpdateBaseDirtiness()
        {
            MeshRenderer.material.SetFloat("_BaseDirtiness", m_BaseDirtiness);
        }

        public void ChangeBaseDirtiness(float amount)
        {
            m_BaseDirtiness = Mathf.Clamp01(m_BaseDirtiness + amount);
            UpdateBaseDirtiness();
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
                DrawApply();
        }

        public void DrawFill(float dirtiness, bool applyChanges = true)
        {
            Color32 col = DirtinessValueToTexCol(dirtiness);

            Color32[] arr = new Color32[DirtinessTextureSizeSqr];
            for (int i = 0; i < arr.Length; i++) arr[i] = col;

            m_DirtinessTexture.SetPixels32(arr);

            if (applyChanges)
                DrawApply();
        }

        protected void DrawRandomDirtSpots(int spotCount, float stdNormalised, bool applyChanges = true)
        {
            float std = stdNormalised * DirtinessTextureSize;
            float variance = std * std;

            float[] buf = new float[DirtinessTextureSizeSqr];

            foreach (var _ in Enumerable.Range(0, spotCount))
            {
                int centerX = Random.Range(0, DirtinessTextureSize);
                int centerY = Random.Range(0, DirtinessTextureSize);

                int maxRange = Mathf.RoundToInt(4 * std); // Enough of the bell curve is captured in this range

                for (int xRaw = centerX - maxRange; xRaw < centerX + maxRange; xRaw++)
                    for (int yRaw = centerY - maxRange; yRaw < centerY + maxRange; yRaw++)
                    {
                        // Wrap the coordinates
                        int x = MathUtils.WrapInt(xRaw, DirtinessTextureSize);
                        int y = MathUtils.WrapInt(yRaw, DirtinessTextureSize);

                        float r = Mathf.Sqrt(Mathf.Pow(x - centerX, 2) + Mathf.Pow(y - centerY, 2)); // Distance from center

                        int i = (y * DirtinessTextureSize) + x; // Index in the buffer

                        float k = Mathf.Exp(-(r * r) / (2 * variance)); // The factor of the spot at the point, based on the Gaussian bell curve

                        buf[i] = Mathf.Clamp01(buf[i] + k); // Draw the value on
                    }
            }

            m_DirtinessTexture.SetPixels32(
                buf
                .Select(DirtinessValueToTexCol)
                .ToArray()
            );

            if (applyChanges)
                DrawApply();
        }

        public void DrawApply()
        {
            m_DirtinessTexture.Apply();
        }
    }
}
