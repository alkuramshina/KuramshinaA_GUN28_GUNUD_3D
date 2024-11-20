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

    private const int FRAME_COUNT = 10;
    private readonly FrameScore[] _frameScore = new FrameScore[FRAME_COUNT];
    private int _currentFrame;
    
    private void Start()
    {
        NextFrame();
    }

    private void Update()
    {
        if (_ball is null || _ball.IsThrown) return;
        
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

    // TODO: there suppose to be 2 throws in a frame
    private void NextFrame()
    {
        _currentFrame++;
        Debug.Log($"Frame: {_currentFrame}");
        
        NewPinSet();
        NewBall();

        var isLast = _currentFrame == FRAME_COUNT;
        var afterStrike = _currentFrame > 1 && _frameScore[_currentFrame - 2].IsStrike;
        var afterSpare = _currentFrame > 1 && _frameScore[_currentFrame - 2].IsSpare;
        
        _frameScore[_currentFrame - 1] = new FrameScore(isLast, afterStrike, afterSpare);
    }

    private void FinishFrame()
    {
        var pinsDown = _pinSet.Pins.Count(pin => pin.IsFallen);
        Debug.Log(_frameScore[_currentFrame - 1].Throw(pinsDown));

        if (_currentFrame > 1)
        {
            _frameScore[_currentFrame - 2].UpdateScoreIfSpare(pinsDown);
            _frameScore[_currentFrame - 2].UpdateScoreIfStrike(pinsDown);
        }
        
        if (_currentFrame == FRAME_COUNT)
        {
            FinishGame();
        }

        NextFrame();
    }

    private void FinishGame()
    {
        Debug.Log("Game Finished");
    }

    private void NewBall()
    {
        if (_ball is not null)
        {
            Destroy(_ball.gameObject);
        }

        _ball = Instantiate(ballPrefab, ballStartPlace.position, Quaternion.identity);
        _ball.OnFinishMovement += FinishFrame;
    }
    
    private void NewPinSet()
    {
        if (_pinSet is not null)
        {
            Destroy(_pinSet.gameObject);
        }

        _pinSet = Instantiate(pinSetPrefab, pinSetPlace.position, Quaternion.identity);
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
}