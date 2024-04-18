using UnityEngine;

namespace BonesVr.Characters.Npcs
{
    public class AnimatedNpcHeadController : MonoBehaviour
    {
        [SerializeField] private bool m_AllowNoHeadSet = false; // Specifically for the recording setup where an instance of this class is used without an attached head

        [SerializeField] private Transform _head;
        protected Transform Head => _head;

        public Vector3 m_Position;
        public Quaternion m_Rotation;

        protected virtual void Awake()
        {
            if (!m_AllowNoHeadSet && Head == null)
                Debug.LogError("No head transform set");
        }

        protected virtual void Update()
        {
            if (Head != null)
                Head.SetPositionAndRotation(m_Position, m_Rotation);
        }
    }
}
