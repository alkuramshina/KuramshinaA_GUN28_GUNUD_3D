using HUD;
using UnityEngine;

namespace Objects
{
    public class Frame
    {
        public Frame(FrameScoreLayout frameScoreLayout,
            bool isLast = false,
            bool afterStrike = false)
        {
            _frameScoreLayout = frameScoreLayout;
        
            if (isLast)
            {
                BonusThrowCount = afterStrike ? BONUS_THROW_COUNT : 0;
            }
        }

        private readonly FrameScoreLayout _frameScoreLayout;
        private const int THROW_COUNT = 2;
        private const int BONUS_THROW_COUNT = 1;

        public int Score { get; private set; }
        public int ThrowCount { get; private set; }
        private int BonusThrowCount { get; set; }

        public bool IsStrike { get; private set; }
        public bool IsSpare { get; private set; }

        public bool CanThrow => ThrowCount < THROW_COUNT
                                || BonusThrowCount > 0;

        
        private const int PIN_COUNT = 10;
        private const int BONUS_STRIKE_SCORE = 10;
        private const int BONUS_SPARE_SCORE = 10;
        public int AddScore(int pinsKnocked)
        {
            if (!CanThrow) return Score;
        
            var currentThrowScore = pinsKnocked;
            var isCurrentStrike = false;
        
            if (ThrowCount == 0)
            {
                IsStrike = currentThrowScore == PIN_COUNT;

                if (IsStrike)
                {
                    Debug.Log("Strike!");
                    currentThrowScore += BONUS_STRIKE_SCORE;
                    isCurrentStrike = true;
                }
            }
            else if (ThrowCount == 1 && !IsStrike)
            {
                IsSpare = currentThrowScore + Score == PIN_COUNT;

                if (IsSpare)
                {
                    Debug.Log("Spare!");
                    currentThrowScore += BONUS_SPARE_SCORE;
                }
            }
            else if (BonusThrowCount > 0)
            {
                BonusThrowCount--;
            }
        
            _frameScoreLayout.SetThrowScore(ThrowCount, currentThrowScore, isCurrentStrike);
        
            ThrowCount++;
            Score += currentThrowScore;
        
            return Score;
        }

        public void SetTotalScore(int totalScore)
        {
            _frameScoreLayout.SetFrameScore(totalScore);
        }

        private const int BONUS_STRIKE_THROW_COUNT = 2;
        private int _strikeScoreUpdatedThrows;
        public bool UpdateScoreIfStrike(int pinsKnockedNextFrame)
        {
            if (!IsStrike || _strikeScoreUpdatedThrows > BONUS_STRIKE_THROW_COUNT) return false;

            Score += pinsKnockedNextFrame;
            _strikeScoreUpdatedThrows++;
            return true;
        }
    
        private const int BONUS_SPARE_THROW_COUNT = 1;
        private int _spareScoreUpdatedThrows;
        public bool UpdateScoreIfSpare(int pinsKnockedNextFrame)
        {
            if (!IsSpare || _spareScoreUpdatedThrows > BONUS_SPARE_THROW_COUNT) return false;

            Score += pinsKnockedNextFrame;
            _spareScoreUpdatedThrows++;
            return true;
        }
    }
}