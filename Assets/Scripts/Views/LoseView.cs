using System;
using Managers;
using UnityEngine;
using Zenject;

namespace Views
{
    public class LoseView : MonoBehaviour
    {
        private AudioManager AudioManager { get; set; }

        [SerializeField] private GameObject losePanel;

        public event Action OnTryAgainClick;
        public event Action OnMenuClick;

        [Inject]
        private void Construct(AudioManager audioManager)
        {
            AudioManager = audioManager;
        }

        public void TryAgain()
        {
            AudioManager.PlayUI();
            OnTryAgainClick?.Invoke();
        }
        public void Menu()
        {
            AudioManager.PlayUI();
            OnMenuClick?.Invoke();
        }
        public void Open() => losePanel.SetActive(true);
        public void Close() => losePanel.SetActive(false);
    }
}