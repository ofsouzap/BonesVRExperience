using UnityEngine;

namespace BonesVr.Utils
{
    public class CopyPositionRotation : MonoBehaviour
    {
        public Transform src;
        public Transform dst;

        protected virtual void Update()
        {
            dst.SetPositionAndRotation(src.position, src.rotation);
        }
    }
}
