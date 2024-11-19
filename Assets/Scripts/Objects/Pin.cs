using System;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pin: MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        [SerializeField] private float velocityThreshold = 10f;
        
        public bool IsMoved { get; private set; }
        public bool IsStopped { get; private set; }
        
        public event Action OnPinDropped;
        
        private void OnCollisionEnter(Collision other)
        {
            IsMoved = true;
            
            if (IsStopped) return;

            if (other.collider.TryGetComponent<Ball>(out var ball)
                || other.collider.TryGetComponent<Pin>(out var otherPin))
            {
                var pinVelocity = _rigidbody.velocity.magnitude;
                if (pinVelocity < velocityThreshold)
                {
                    IsStopped = true;
                    OnPinDropped?.Invoke();
                }
            }
        }
    }
}