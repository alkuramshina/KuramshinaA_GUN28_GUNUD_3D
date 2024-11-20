using UnityEngine;

namespace HUD
{
    public class HudController: MonoBehaviour
    {
        [SerializeField] private EscapeMenu escapeMenu;

        private void Awake()
        {
            escapeMenu.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                escapeMenu.gameObject.SetActive(!escapeMenu.isActiveAndEnabled);
            }
        }
    }
}