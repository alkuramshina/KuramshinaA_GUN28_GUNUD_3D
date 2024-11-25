using HUD;
using UnityEngine;

public class Frame
{
    public Frame(FrameScoreLayout frameScoreLayout,
        bool isLast = false,
        bool afterStrike = false)
    {
        _frameScoreLayout = frameScoreLayout;
        
        if (isLast)
        {
            BonusThrowCount = afterStrike ? 1 : 0;
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
        
        var currentThrowScore = pinsKnocked;
        var isCurrentStrike = false;
        
        if (ThrowCount == 0)
        {
            IsStrike = currentThrowScore == 10;

            if (IsStrike)
            {
                Debug.Log("Strike!");
                currentThrowScore += 10;
                isCurrentStrike = true;
            }
        }
        else if (ThrowCount == 1 && !IsStrike)
        {
            IsSpare = currentThrowScore + Score == 10;

            if (IsSpare)
            {
                Debug.Log("Spare!");
                currentThrowScore += 10;
            }
        }
        else if (BonusThrowCount > 0)
        {
            BonusThrowCount--;
            Debug.Log($"Bonus throws: {BonusThrowCount}");
        }
        
        _frameScoreLayout.SetThrowScore(ThrowCount, currentThrowScore, isCurrentStrike);
        
        ThrowCount++;
        Score += currentThrowScore;
        
        Debug.Log($"Throws: {ThrowCount}");
        Debug.Log($"Score: {Score}");
        
        return Score;
    }

    public void SetTotalScore(int totalScore)
    {
        _frameScoreLayout.SetFrameScore(totalScore);
    }

    private int _strikeScoreUpdatedThrows;
    public bool UpdateScoreIfStrike(int pinsKnockedNextFrame)
    {
        if (!IsStrike || _strikeScoreUpdatedThrows > 2) return false;

        Score += pinsKnockedNextFrame;
        _strikeScoreUpdatedThrows++;
        return true;
    }
    
    private int _spareScoreUpdatedThrows;
    public bool UpdateScoreIfSpare(int pinsKnockedNextFrame)
    {
        if (!IsSpare || _spareScoreUpdatedThrows > 1) return false;

        Score += pinsKnockedNextFrame;
        _spareScoreUpdatedThrows++;
        return true;
    }
}