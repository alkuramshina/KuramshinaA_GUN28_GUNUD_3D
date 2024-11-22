using System;
using Objects;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    [RequireComponent(typeof(Button))]
    public class BallButton: MonoBehaviour
    {
        public BallTypeSO BallType;
        public event Action<BallTypeSO> OnClick;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            Debug.Log($"Clicked: {BallType.type}");
            OnClick?.Invoke(BallType);
        }
    }
}