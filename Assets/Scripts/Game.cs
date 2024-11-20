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
    private int _currentFrameNumber;
    private readonly Frame[] _frames = new Frame[FRAME_COUNT];
    private Frame CurrentFrame => _frames[_currentFrameNumber - 1];
    
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
    
    private void NextFrame()
    {
        _currentFrameNumber++;
        Debug.Log($"Frame: {_currentFrameNumber}");
        
        NewPinSet();
        NewBall();

        var isLast = _currentFrameNumber == FRAME_COUNT;
        var afterStrike = _currentFrameNumber > 1 && _frames[_currentFrameNumber - 2].IsStrike;
        var afterSpare = _currentFrameNumber > 1 && _frames[_currentFrameNumber - 2].IsSpare;
        
        _frames[_currentFrameNumber - 1] = new Frame(isLast, afterStrike, afterSpare);
    }

    private void FinishThrow()
    {
        var pinsDown = _pinSet.Pins.Count(pin => pin.IsFallen);
        Debug.Log(CurrentFrame.Throw(pinsDown));

        // if there is a previous frame
        if (_currentFrameNumber - 2 > 0)
        {
            _frames[_currentFrameNumber - 2].UpdateScoreIfSpare(pinsDown);
            _frames[_currentFrameNumber - 2].UpdateScoreIfStrike(pinsDown);

            if (_currentFrameNumber - 3 > 0)
            {
                _frames[_currentFrameNumber - 3].UpdateScoreIfStrike(pinsDown);
            }
        }

        // if it was the last frame
        if (_currentFrameNumber == FRAME_COUNT)
        {
            FinishGame();
        }

        if (CurrentFrame.CanThrow)
        {
            NewBall();
        }
        else
        {
            NextFrame();
        }
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
        _ball.OnFinishMovement += FinishThrow;
    }
    
    private void NewPinSet()
    {
        if (_pinSet is not null)
        {
            foreach (var pin in _pinSet.Pins)
            {
                Destroy(pin.gameObject);
            }

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