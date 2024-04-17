using UnityEngine;

namespace BonesVr.Ui
{
    public class SimpleBillboard : MonoBehaviour
    {
        [SerializeField] private bool _oppositeDirection;
        protected bool OppositeDirection => _oppositeDirection;

        protected virtual void Update()
        {
            Transform camera = Camera.main.transform;
            transform.rotation = Quaternion.LookRotation((OppositeDirection ? -1 : 1) * (camera.position - transform.position));
        }
    }
}
