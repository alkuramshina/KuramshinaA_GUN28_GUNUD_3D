using HUD;
using UnityEngine;

public class Frame
{
    public Frame(FrameScoreLayout frameScoreLayout,
        bool isLast = false,
        bool afterStrike = false,
        bool afterSpare = false)
    {
        _frameScoreLayout = frameScoreLayout;
        
        if (isLast)
        {
            BonusThrowCount = afterStrike ? 2 : afterSpare ? 1 : 0;
            Debug.Log($"Bonus throws: {BonusThrowCount}");
        }
    }

    private readonly FrameScoreLayout _frameScoreLayout;

    public int Score { get; private set; }
    public int ThrowCount { get; private set; }
    private int BonusThrowCount { get; set; }

    public bool IsStrike { get; private set; }
    public bool IsSpare { get; private set; }

    public bool CanThrow => ThrowCount < 2
                            || BonusThrowCount > 0;

    public int AddScore(int pinsKnocked)
    {
        if (!CanThrow) return Score;
        
        var throwScore = pinsKnocked;
        
        if (ThrowCount == 0)
        {
            IsStrike = throwScore == 10;

            if (IsStrike)
            {
                Debug.Log("Strike!");
                throwScore += 10;
            }
        }
        else if (ThrowCount == 1 && !IsStrike)
        {
            IsSpare = throwScore == 10;

            if (IsSpare)
            {
                Debug.Log("Spare!");
                throwScore += 10;
            }
        }
        else if (!IsSpare && BonusThrowCount > 0)
        {
            BonusThrowCount--;
            Debug.Log($"Bonus throws: {BonusThrowCount}");
        }
        else
        {
            _frameScoreLayout.SetThrowScore(2, 0);
            _frameScoreLayout.SetFrameScore(Score);
        }
        
        _frameScoreLayout.SetThrowScore(ThrowCount, throwScore);
        
        ThrowCount++;
        Score += throwScore;
        
        Debug.Log($"Throws: {ThrowCount}");
        Debug.Log($"Score: {Score}");
        
        return Score;
    }

    private int _strikeScoreUpdatedFrames;
    public void UpdateScoreIfStrike(int pinsKnockedNextFrame)
    {
        if (!IsStrike || _strikeScoreUpdatedFrames > 2) return;

        Score += pinsKnockedNextFrame;
        _strikeScoreUpdatedFrames++;
        
        _frameScoreLayout.SetFrameScore(Score);
    }
    
    private int _spareScoreUpdatedFrames;
    public void UpdateScoreIfSpare(int pinsKnockedNextFrame)
    {
        if (!IsSpare || _spareScoreUpdatedFrames > 1) return;

        Score += pinsKnockedNextFrame;
        _spareScoreUpdatedFrames++;
        
        _frameScoreLayout.SetFrameScore(Score);
    }
}