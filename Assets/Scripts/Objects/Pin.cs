using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pin: MonoBehaviour
    {
        private float PinAngle => Vector3.Dot(transform.up, Vector3.up);

        public bool IsFallen => PinAngle < 1;
    }
}