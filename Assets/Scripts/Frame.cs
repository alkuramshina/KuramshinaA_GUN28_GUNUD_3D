public class Frame
{
    public Frame(bool isLast = false,
        bool afterStrike = false,
        bool afterSpare = false)
    {
        if (isLast)
        {
            _bonusThrows = afterStrike ? 2 : afterSpare ? 1 : 0;
        }
    }

    private readonly int _bonusThrows = 0;

    private int _score;
    private int _throwCount;

    private bool _isStrike;
    private bool _isSpare;

    public int Throw(int pinsKnocked)
    {
        if (_throwCount == 0)
        {
            _score += pinsKnocked;
            _isStrike = _score == 10;

            if (_isStrike)
            {
                _score += 10;
            }

            _throwCount++;
        }
        else if (_throwCount == 1 && !_isStrike)
        {
            _score += pinsKnocked;
            _isSpare = _score == 10;

            if (_isSpare)
            {
                _score += 10;
            }

            _throwCount++;
        }
        else if (!_isSpare
                 && _bonusThrows > 0
                 && _throwCount + _bonusThrows <= 2 + _bonusThrows)
        {
            _score += pinsKnocked;
            _throwCount++;
        }

        return _score;
    }

    private int _strikeScoreUpdatedFrames;
    public void UpdateStrikeScore(int pinsKnockedNextFrame)
    {
        if (!_isStrike || _strikeScoreUpdatedFrames > 2) return;

        _score += pinsKnockedNextFrame;
        _strikeScoreUpdatedFrames++;
    }
    
    private int _spareScoreUpdatedFrames;
    public void UpdateSpareScore(int pinsKnockedNextFrame)
    {
        if (!_isSpare || _spareScoreUpdatedFrames > 1) return;

        _score += pinsKnockedNextFrame;
        _spareScoreUpdatedFrames++;
    }
}