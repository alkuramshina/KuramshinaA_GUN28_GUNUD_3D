using System;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball: MonoBehaviour
    {
        public event Action OnFinishMovement;
        
        public bool IsThrown { get; private set; }
        public bool IsStopped { get; private set; }
        public bool IsCollided { get; private set; }
        
        private Rigidbody _rigidbody;
        
        [SerializeField] private float magnitudeThreshold = .2f;

        private void Awake()
        {
            IsThrown = false;
            IsCollided = false;
            IsStopped = false;
            
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!IsThrown || IsStopped || !IsCollided) return;
            if (_rigidbody.velocity.magnitude <= magnitudeThreshold)
            {
                IsStopped = true;
                OnFinishMovement?.Invoke();
            }
        }

        public void Throw(Vector3 velocity)
        {
            IsThrown = true;
            
            _rigidbody.AddForce(velocity, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (IsCollided) return;
            if (other.collider.TryGetComponent(out Pin pin))
            {
                IsCollided = true;
            }
        }
    }
}