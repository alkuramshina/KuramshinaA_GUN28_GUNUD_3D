using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball: MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public void Push(float force)
        {
            _rigidbody.AddForce(Vector3.forward * force, ForceMode.Impulse);
        }
    }
}