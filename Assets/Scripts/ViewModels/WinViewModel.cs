using System;
using Managers;
using Models;
using Zenject;

namespace ViewModels
{
    public class WinViewModel : IInitializable
    {
        public event Action<float> OnTimerChange;
        private WinModel WinModel { get; set; }
        private StatManager StatManager { get; set; }
        private GameControlModel GameModel { get; set; }
        private MenuViewModel MenuViewModel { get; set; }

        public event Action<int> OnShowWinScreen;
        public event Action OnCloseWinScreen;

        [Inject]
        private void Construct(WinModel winModel, StatManager statManager, GameControlModel gameModel, MenuViewModel menuViewModel)
        {
            WinModel = winModel;
            StatManager = statManager;
            GameModel = gameModel;
            MenuViewModel = menuViewModel;
        }

        public void Initialize()
        {
            WinModel.OnTimerChange += TimerDataChanged;
            WinModel.OnWin += ShowWinScreen;
        }

        private void ShowWinScreen()
        {
            OnShowWinScreen?.Invoke(WinModel.GetStars());
        }

        public string GetTimer() => WinModel.GetTimer();
        public string GetBestScore() => WinModel.GetBestScore();
        public string GetEnemiesKilled() => StatManager.EnemiesKilled.ToString();
        public string GetDefensesDestroyed() => StatManager.DefensesDestroyed.ToString();

        public void NextLevel()
        {
            GameModel.Restart();
            OnCloseWinScreen?.Invoke();
        }

        public void Menu()
        {
            MenuViewModel.OpenMenu();
            NextLevel();
        }

        private void TimerDataChanged() => OnTimerChange?.Invoke(WinModel.Timer);
    }
}