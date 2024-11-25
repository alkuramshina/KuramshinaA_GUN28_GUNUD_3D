using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Objects
{
    public class PinSet : MonoBehaviour
    {
        [SerializeField] private List<Pin> pins;

        public int FallenCount => pins.Count(pin => pin.IsFallen);

        private void OnDestroy()
        {
            foreach (var pin in pins)
            {
                Destroy(pin.gameObject);
            }
            
            Destroy(gameObject);
        }
    }
}