using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views
{
    public class DefenseView : MonoBehaviour
    {
        private AudioManager AudioManager { get; set; }

        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI price;

        public event Action OnDefenseSelected;

        public void Init(string Price, Sprite Image, AudioManager audioManager)
        {
            AudioManager = audioManager;
            price.text = Price;
            image.sprite = Image;
            button.onClick.AddListener(SelectDefence);
        }

        public void UpdateButton(bool active) => button.interactable = active;

        private void SelectDefence()
        {
            AudioManager.PlayUI();
            OnDefenseSelected?.Invoke();
        }
    }
}