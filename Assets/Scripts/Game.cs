using System;
using System.Collections;
using HUD;
using Objects;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PinSet pinSetPrefab;
    [SerializeField] private Transform pinSetPlace;
    [SerializeField] private BallTypeSO defaultBallType;
    
    [SerializeField] private UIController uiController;

    [SerializeField] private float frameDelay = .5f;
    [SerializeField] private PlayerControls playerControls;
    
    private BallTypeSO _currentBallType;
    private Ball _ball;
    private PinSet _pinSet;
    
    private float _holdDownStartTime;

    private const int FRAME_COUNT = 10;
    private int _currentFrameNumber;
    
    private readonly Frame[] _frames = new Frame[FRAME_COUNT];
    private bool IsLastFrame => _currentFrameNumber == FRAME_COUNT;
    private Frame CurrentFrame => _frames[_currentFrameNumber - 1];
    private Frame PreviousFrame => _frames[_currentFrameNumber - 2];

    private int _totalScore;
    
    private void Start()
    {
        uiController.SetBallsToChange(ChangeBall);
        uiController.UpdateFrameText(1, 1);
        uiController.SetFrameScoreTextsToDefault();

        _currentFrameNumber = 1;
        _currentBallType = defaultBallType;
        
        NextFrame();
    }
    
    private void Update()
    {
        if (playerControls.State == PlayerControls.ControlState.Thrown && !_ball.IsThrown)
        { 
            _ball.Throw(playerControls.ChosenVelocity);
        }
    }
    
    private void NextFrame()
    {
        if (_currentFrameNumber > 1)
        {
            _totalScore += PreviousFrame.Score;
            PreviousFrame.SetTotalScore(_totalScore);
        }

        NewPinSet();
        NewBall();

        var afterStrike = _currentFrameNumber > 1 && PreviousFrame.IsStrike;

        _frames[_currentFrameNumber - 1] = new Frame(uiController.FrameScoreTexts[_currentFrameNumber - 1], 
            IsLastFrame,
            afterStrike);
    }

    private void FinishThrow()
    {
        var pinsPreviouslyDown = CurrentFrame.IsStrike || CurrentFrame.IsSpare
            ? 0
            : CurrentFrame.Score;
        var pinsDown = _pinSet.FallenCount - pinsPreviouslyDown;
        
        CurrentFrame.AddScore(pinsDown);
            
        // if there is a previous frame
        if (_currentFrameNumber > 1)
        {
            if (PreviousFrame.UpdateScoreIfSpare(pinsDown)) _totalScore += pinsDown;
            if (PreviousFrame.UpdateScoreIfStrike(pinsDown)) _totalScore += pinsDown;
            
            PreviousFrame.SetTotalScore(_totalScore);
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
        CurrentFrame.SetTotalScore(_totalScore);
        Debug.Log($"Game Finished. Total score is {_totalScore}");
    }

    private void ChangeBall(BallTypeSO ballType)
    {
        if (_ball.IsThrown) return;
        
        _currentBallType = ballType;
        NewBall();
        uiController.CloseEscapeMenu();
    }

    private void NewBall()
    {
        if (_ball is not null)
        {
            Destroy(_ball.gameObject);
        }

        StartCoroutine(DelayAction(() =>
        {
            _ball = Instantiate(_currentBallType.prefab, playerControls.BallStartPosition, Quaternion.identity);
            _ball.OnFinishMovement += FinishThrow;
            
            playerControls.SetReady();
        }));
    }
    
    private void NewPinSet()
    {
        if (_pinSet is not null)
        {
            Destroy(_pinSet.gameObject);
        }

        StartCoroutine(DelayAction(() =>
        {
            _pinSet = Instantiate(pinSetPrefab, pinSetPlace.position, Quaternion.identity);
        }));
    }

    private IEnumerator DelayAction(Action action)
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