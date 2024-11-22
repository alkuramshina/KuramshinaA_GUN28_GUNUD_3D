using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace HUD
{
    public class UIController: MonoBehaviour
    {
        [SerializeField] private EscapeMenu escapeMenu;
        public List<BallButton> BallsToChoose;

        private void Awake()
        {
            escapeMenu.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                SetEscapeMenu(!escapeMenu.isActiveAndEnabled);
            }
        }

        public void SetEscapeMenu(bool isActive)
        {
            escapeMenu.gameObject.SetActive(isActive);
        }
    }
}