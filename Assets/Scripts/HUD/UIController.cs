using System;
using System.Collections.Generic;
using Audio;
using Objects;
using TMPro;
using UnityEngine;

namespace HUD
{
    public class UIController: MonoBehaviour
    {
        [SerializeField] private EscapeMenu escapeMenu;
        [SerializeField] private List<BallButton> ballsToChoose;
        [SerializeField] private TMP_Text frameText;
        [SerializeField] private AudioController audioController;
        
        public FrameScoreLayout[] FrameScoreTexts;
        
        public void SetBallsToChange(Action<BallTypeSO> onChange)
        {
            foreach (var ballToChoose in ballsToChoose)
            {
                ballToChoose.OnClick += onChange;
            }
        }

        public void UpdateFrameText(int frameNumber, int throwNumber)
        {
            frameText.text = $"Frame: {frameNumber}\nThrow: {throwNumber}";
        }
        
        public void SetFrameScoreTextsToDefault()
        {
            foreach (var frameScoreText in FrameScoreTexts)
            {
                frameScoreText.SetToDefault();
            }
        }

        public void CloseEscapeMenu() => SetEscapeMenu(false);
        public void OpenEscapeMenu() => SetEscapeMenu(true);
        
        private void Awake()
        {
            SetEscapeMenu(false);
            foreach (var ballToChoose in ballsToChoose)
            {
                ballToChoose.OnClick += _ => {audioController.PlayBallChoosingSound();};
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                audioController.PlayMenuSound();
                SetEscapeMenu(!escapeMenu.isActiveAndEnabled);
            }
        }

        private void SetEscapeMenu(bool isActive)
        {
            escapeMenu.gameObject.SetActive(isActive);
            
            Time.timeScale = isActive ? 0 : 1;
        }
    }
}