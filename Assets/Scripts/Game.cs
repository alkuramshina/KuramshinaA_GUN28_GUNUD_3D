using System;
using System.Collections;
using HUD;
using Objects;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PinSet pinSetPrefab;
    [SerializeField] private Transform pinSetPlace;
    [SerializeField] private BallTypeSO defaultBall;
    
    [SerializeField] private Transform ballStartPlace;
    [SerializeField] private UIController uiController;

    [SerializeField] private float frameDelay = .5f;
    
    private BallTypeSO _currentBall;
    private Ball _ball;
    private PinSet _pinSet;
    
    private float _holdDownStartTime;

    private const int FRAME_COUNT = 10;
    private int _currentFrameNumber;
    
    private readonly Frame[] _frames = new Frame[FRAME_COUNT];
    private bool IsLastFrame => _currentFrameNumber == FRAME_COUNT;
    private Frame CurrentFrame => _frames[_currentFrameNumber - 1];
    private Frame PreviousFrame => _frames[_currentFrameNumber - 2];
    private Frame PreviousPreviousFrame => _frames[_currentFrameNumber - 3];
    
    private void Start()
    {
        uiController.SetBallsToChange(ChangeBall);
        uiController.UpdateFrameText(1, 1);

        _currentFrameNumber = 1;
        _currentBall = defaultBall;
        NextFrame();
    }

    private void Update()
    {
        if (_ball is null|| _pinSet is null 
                         || _ball.IsThrown
                         || uiController.EscapeMenuIsOpen) return;
        
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
        NewPinSet();
        NewBall();

        var afterStrike = _currentFrameNumber > 1 && PreviousFrame.IsStrike;
        var afterSpare = _currentFrameNumber > 1 && PreviousFrame.IsSpare;

        _frames[_currentFrameNumber - 1] = new Frame(IsLastFrame, afterStrike, afterSpare);
    }

    private void FinishThrow()
    {
        Debug.Log($"Fallen: { _pinSet.FallenCount}");

        var pinsPreviouslyDown = CurrentFrame.IsStrike || CurrentFrame.IsSpare
            ? 0
            : CurrentFrame.Score;
        var pinsDown = _pinSet.FallenCount - pinsPreviouslyDown;
        
        CurrentFrame.AddScore(pinsDown);
            
        // if there is a previous frame
        if (_currentFrameNumber - 2 > 0)
        {
            PreviousFrame.UpdateScoreIfSpare(pinsDown);
            PreviousFrame.UpdateScoreIfStrike(pinsDown);

            // if there is a frame before previous
            if (_currentFrameNumber - 3 > 0)
            {
                PreviousPreviousFrame.UpdateScoreIfStrike(pinsDown);
            }
        }

        if (CurrentFrame.CanThrow)
        {
            NewBall();
            uiController.UpdateFrameText(_currentFrameNumber, CurrentFrame.ThrowCount + 1);

            if (CurrentFrame.IsStrike)
            {
                NewPinSet();
            }
        }
        else if (IsLastFrame)
        {
            FinishGame();
        }
        else
        {
            _currentFrameNumber++;
            uiController.UpdateFrameText(_currentFrameNumber, 1);
            
            NextFrame();
        }
    }

    private void FinishGame()
    {
        Debug.Log("Game Finished");
    }

    private void ChangeBall(BallTypeSO ballType)
    {
        if (_ball.IsThrown) return;
        
        _currentBall = ballType;
        NewBall();
        uiController.CloseEscapeMenu();
    }

    private void NewBall()
    {
        if (_ball is not null)
        {
            Destroy(_ball.gameObject);
        }

        StartCoroutine(FrameDelay(() =>
        {
            _ball = Instantiate(_currentBall.prefab, ballStartPlace.position, Quaternion.identity);
            _ball.OnFinishMovement += FinishThrow;
        }));
    }
    
    private void NewPinSet()
    {
        if (_pinSet is not null)
        {
            Destroy(_pinSet.gameObject);
        }

        StartCoroutine(FrameDelay(() =>
        {
            _pinSet = Instantiate(pinSetPrefab, pinSetPlace.position, Quaternion.identity);
        }));
    }

    private IEnumerator FrameDelay(Action action)
    {
        yield return new WaitForSeconds(frameDelay);
        action?.Invoke();
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