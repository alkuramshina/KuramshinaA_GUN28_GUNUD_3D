using System.Linq;
using Objects;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PinSet pinSetPrefab;
    [SerializeField] private Transform pinSetPlace;
    
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Transform ballStartPlace;

    private Ball _ball;
    private PinSet _pinSet;
    
    private float _holdDownStartTime;
    
    private void Start()
    {
        NewRound();
    }

    private void Update()
    {
        if (_ball is null || _ball.IsMoving) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _holdDownStartTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            var holdDownTime = Time.time - _holdDownStartTime;
            _ball.Throw(ballStartPlace.forward * CalculateHoldDownForce(holdDownTime));
        }
    }

    private void FinishRound()
    {
        Debug.Log("Finish");
        Debug.Log($"{_pinSet.Pins.Count(pin => pin.IsMoved && pin.IsStopped)} pins dropped");
    }

    private void NewRound()
    {
        NewPinSet();
        NewBall();
    }
    
    private const float MaxForce = 200f;
    private const float MinForce = 100f;
    private const float MaxForceHoldDownTime = 3f;
    private static float CalculateHoldDownForce(float holdTime)
    {
        var holdTimeNormalized = Mathf.Clamp01(holdTime / MaxForceHoldDownTime);
        var force = holdTimeNormalized * MaxForce;      
        
        return Mathf.Clamp(force, MinForce, MaxForce);
    }

    private void NewBall()
    {
        if (_ball is not null)
        {
            Destroy(_ball.gameObject);
        }

        _ball = Instantiate(ballPrefab, ballStartPlace.position, Quaternion.identity);
    }
    
    private void NewPinSet()
    {
        if (_pinSet is not null)
        {
            Destroy(_pinSet.gameObject);
        }

        _pinSet = Instantiate(pinSetPrefab, pinSetPlace.position, Quaternion.identity);
        
        foreach (var pin in _pinSet.Pins)
        {
            pin.OnPinDropped += CheckIfPinsStopped;
        }
    }

    private void CheckIfPinsStopped()
    {
        if (_pinSet.Pins.All(pin => !pin.IsMoved || pin.IsStopped))
        {
            FinishRound();
        }
    }
}