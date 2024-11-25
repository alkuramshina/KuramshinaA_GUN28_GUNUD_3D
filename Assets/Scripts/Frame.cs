using UnityEngine;

public class Frame
{
    public Frame(bool isLast = false,
        bool afterStrike = false,
        bool afterSpare = false)
    {
        if (isLast)
        {
            BonusThrows = afterStrike ? 2 : afterSpare ? 1 : 0;
        }
    }

    private int BonusThrows { get; set; }

    public int Score { get; private set; }
    private int ThrowCount { get; set; }

    public bool IsStrike { get; private set; }
    public bool IsSpare { get; private set; }

    public bool CanThrow => ThrowCount < 2
                            || BonusThrows > 0;

    public int Throw(int pinsKnocked)
    {
        if (ThrowCount == 0)
        {
            Score += pinsKnocked;
            IsStrike = Score == 10;

            if (IsStrike)
            {
                Debug.Log("Strike!");
                Score += 10;
            }

            ThrowCount++;
        }
        else if (ThrowCount == 1 && !IsStrike)
        {
            Score += pinsKnocked;
            IsSpare = Score == 10;

            if (IsSpare)
            {
                Debug.Log("Spare!");
                Score += 10;
            }

            ThrowCount++;
        }
        else if (!IsSpare && BonusThrows > 0)
        {
            Score += pinsKnocked;
            ThrowCount++;
            BonusThrows--;
        }

        
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