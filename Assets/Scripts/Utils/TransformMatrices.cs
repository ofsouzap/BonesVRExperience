using UnityEngine;

namespace BonesVr.Utils
{
    public readonly struct TRS
    {
        public readonly Vector3 position;
        public readonly Quaternion rotation;
        public readonly Vector3 scale;

        public TRS(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public static TRS FromTransform(Transform transform)
            => new(transform.position, transform.rotation, transform.lossyScale);

        public static TRS Identity => new(Vector3.zero, Quaternion.identity, Vector3.one);
    }

    public static class TransformMatrices
    {
        public static Matrix4x4 FromTrs(Vector3 position, Quaternion rotation, Vector3 scale)
            => Matrix4x4.TRS(position, rotation, scale);

        public static Matrix4x4 FromTrs(TRS trs)
            => FromTrs(trs.position, trs.rotation, trs.scale);

        public static Matrix4x4 FromTransform(Transform transform)
            => FromTrs(transform.position, transform.rotation, transform.lossyScale);

        public static TRS ToTrs(Matrix4x4 mat)
        {
            // See https://discussions.unity.com/t/how-to-decompose-a-trs-matrix/63681

            Vector3 position;
            Quaternion rotation;
            Vector3 scale;

            position = mat.GetColumn(3);
            rotation = Quaternion.LookRotation(mat.GetColumn(2), mat.GetColumn(1));
            scale = new(
                mat.GetColumn(0).magnitude,
                mat.GetColumn(1).magnitude,
                mat.GetColumn(2).magnitude
            );

            return new(position, rotation, scale);
        }

        public static void ToTrs(Matrix4x4 mat, out Vector3 position, out Quaternion rotation, out Vector3 scale)
        {
            TRS trs = ToTrs(mat);
            position = trs.position;
            rotation = trs.rotation;
            scale = trs.scale;
        }
    }
}
