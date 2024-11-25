using TMPro;
using UnityEngine;

namespace HUD
{
    public class FrameScoreLayout: MonoBehaviour
    {
        [SerializeField] private TMP_Text totalScoreText;
        [SerializeField] private TMP_Text[] throwScoreTexts;

        public void SetThrowScore(int throwIndex, int score)
        {
            if (throwIndex >= throwScoreTexts.Length) return;    
            
            throwScoreTexts[throwIndex].text = score > 0 ? $"{score}" : "/";
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