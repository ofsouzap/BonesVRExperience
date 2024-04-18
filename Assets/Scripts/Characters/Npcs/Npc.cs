using BonesVr.Characters.Hands;
using UnityEngine;

namespace BonesVr.Characters.Npcs
{
    public class Npc : MonoBehaviour
    {
        [SerializeField] private Hand _rightHand;
        public Hand RightHand => _rightHand;

        [SerializeField] private Hand _leftHand;
        public Hand LeftHand => _leftHand;

        [SerializeField] private GameObject _head;
        public GameObject Head => _head;

        protected virtual void Awake()
        {
            if (RightHand == null)
                Debug.LogError("No right hand set");

            if (LeftHand == null)
                Debug.LogError("No left hand set");
            
            if (Head == null)
                Debug.LogError("No head set");
        }
    }
}
