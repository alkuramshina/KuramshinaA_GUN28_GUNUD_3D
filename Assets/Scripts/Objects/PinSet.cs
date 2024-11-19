using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Objects
{
    public class PinSet: MonoBehaviour
    {
        private List<Pin> _pins;

        private void Awake()
        {
            _pins = GetComponents<Pin>().ToList();
        }
    }
}