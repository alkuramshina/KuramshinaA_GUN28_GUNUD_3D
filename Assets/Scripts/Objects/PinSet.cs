using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Objects
{
    public class PinSet: MonoBehaviour
    {
        public List<Pin> Pins;

        private void Awake()
        {
            Pins = GetComponents<Pin>().ToList();
        }

        private void OnDestroy()
        {
            if (Pins is null) return;
            
            foreach (var pin in Pins)
            {
                Destroy(pin);
            }
        }
    }
}