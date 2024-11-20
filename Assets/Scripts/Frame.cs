public class FrameScore
{
    public FrameScore(bool isLast = false,
        bool afterStrike = false,
        bool afterSpare = false)
    {
        if (isLast)
        {
            _bonusThrows = afterStrike ? 2 : afterSpare ? 1 : 0;
        }
    }

    private readonly int _bonusThrows = 0;

    public int Score { get; private set; }
    public int ThrowCount { get; private set; }

    public bool IsStrike { get; private set; }
    public bool IsSpare { get; private set; }

    public int Throw(int pinsKnocked)
    {
        if (ThrowCount == 0)
        {
            Score += pinsKnocked;
            IsStrike = Score == 10;

            if (IsStrike)
            {
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
                Score += 10;
            }

            ThrowCount++;
        }
        else if (!IsSpare
                 && _bonusThrows > 0
                 && ThrowCount + _bonusThrows <= 2 + _bonusThrows)
        {
            Score += pinsKnocked;
            ThrowCount++;
        }

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