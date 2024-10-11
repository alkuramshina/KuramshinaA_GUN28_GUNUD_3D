using Objects;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PinSet pinSetPrefab;
    [SerializeField] private Transform pinSetPlace;
    
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Transform ballStartPlace;

    private Frame Frame()
    {
        // instantiate pin set in place
        // instantiate ball in place

        return new Frame();
    }

    private void Calculate()
    {
    }
}