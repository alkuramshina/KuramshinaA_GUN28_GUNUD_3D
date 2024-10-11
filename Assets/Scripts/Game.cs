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
    private bool _isBallMoving;
    
    private void Start()
    {
        NewBall();
        _pinSet = Instantiate(pinSetPrefab, pinSetPlace.position, Quaternion.identity);
    }

    private void Update()
    {
        if (!_isBallMoving)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _holdDownStartTime = Time.time;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                var holdDownTime = Time.time - _holdDownStartTime;
                _ball.Push(CalculateHoldDownForce(holdDownTime));
            }
        }
    }
    
    
    private const float MaxForce = 80f;
    private const float MinForce = 30f;
    private const float MaxForceHoldDownTime = 2f;
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
            Destroy(_ball);
        }

        _ball = Instantiate(ballPrefab, ballStartPlace.position, Quaternion.identity);
    }
}