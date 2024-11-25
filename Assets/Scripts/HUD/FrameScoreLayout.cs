using TMPro;
using UnityEngine;

namespace HUD
{
    public class FrameScoreLayout: MonoBehaviour
    {
        [SerializeField] private TMP_Text totalScoreText;
        [SerializeField] private TMP_Text[] throwScoreTexts;

        public void SetThrowScore(int throwIndex, int score, bool isStrike)
        {
            if (throwIndex >= throwScoreTexts.Length || score == 0) return;

            throwScoreTexts[throwIndex].text = isStrike
                ? "X"
                : score >= 10
                    ? "/"
                    : $"{score}";
        }

        public void SetToDefault()
        {
            totalScoreText.text = "-";
            foreach (var throwScoreText in throwScoreTexts)
            {
                throwScoreText.text = "-";
            }
        }

        public void SetFrameScore(int score)
        {
            totalScoreText.text = $"{score}";
        }
    }
}