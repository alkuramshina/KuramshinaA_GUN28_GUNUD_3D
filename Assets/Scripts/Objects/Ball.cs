using System;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball: MonoBehaviour
    {
        public Action OnFinishMovement;
        public bool IsMoving { get; private set; } 
        
        private Rigidbody _rigidbody;

        private void Awake()
        {
            IsMoving = false;
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public void Throw(Vector3 velocity)
        {
            IsMoving = true;
            _rigidbody.AddForce(velocity, ForceMode.Impulse);
        }
    }
}