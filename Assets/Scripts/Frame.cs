using UnityEngine;

public class Frame
{
    public Frame(bool isLast = false,
        bool afterStrike = false,
        bool afterSpare = false)
    {
        if (isLast)
        {
            BonusThrowCount = afterStrike ? 2 : afterSpare ? 1 : 0;
            Debug.Log($"Bonus throws: {BonusThrowCount}");
        }
    }

    public int Score { get; private set; }
    private int ThrowCount { get; set; }
    private int BonusThrowCount { get; set; }

    public bool IsStrike { get; private set; }
    public bool IsSpare { get; private set; }

    public bool CanThrow => ThrowCount < 2
                            || BonusThrowCount > 0;

    public int AddScore(int pinsKnocked)
    {
        if (!CanThrow) return Score;
        
        Score += pinsKnocked;
        
        if (ThrowCount == 0)
        {
            IsStrike = Score == 10;

            if (IsStrike)
            {
                Debug.Log("Strike!");
                Score += 10;
            }
        }
        else if (ThrowCount == 1 && !IsStrike)
        {
            IsSpare = Score == 10;

            if (IsSpare)
            {
                Debug.Log("Spare!");
                Score += 10;
            }
        }
        else if (!IsSpare && BonusThrowCount > 0)
        {
            BonusThrowCount--;
            Debug.Log($"Bonus throws: {BonusThrowCount}");
        }
        
        ThrowCount++;
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
    }
    
    private int _spareScoreUpdatedFrames;
    public void UpdateScoreIfSpare(int pinsKnockedNextFrame)
    {
        if (!IsSpare || _spareScoreUpdatedFrames > 1) return;

        Score += pinsKnockedNextFrame;
        _spareScoreUpdatedFrames++;
    }
}