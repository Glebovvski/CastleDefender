using Defendable;
using Grid;
using Managers;
using UnityEngine;
using Zenject;

namespace Models
{
    public class GameTimeModel : IInitializable, ITickable
    {
        private DefensesModel DefensesModel { get; set; }
        private GameGrid Grid { get; set; }
        private InputManager InputManager { get; set; }
        private CastleDefense Castle { get; set; }
        private CurrencyModel CurrencyModel { get; set; }
        private WinModel WinModel { get; set; }
        private AdManager AdManager { get; set; }

        private float lastDropTime = 0;
        private float secondsToDropGold = 5f;
        private float goldPercent = 2f;

        public bool IsPaused = Time.timeScale < 1;
        private bool isWon = false;
        private bool gridCreated = false;

        [Inject]
        private void Construct(DefensesModel defensesModel, GameGrid grid, InputManager inputManager, CastleDefense castle, CurrencyModel currencyModel, WinModel winModel, AdManager adManager)
        {
            DefensesModel = defensesModel;
            Grid = grid;
            InputManager = inputManager;
            Castle = castle;
            CurrencyModel = currencyModel;
            WinModel = winModel;
            AdManager = adManager;
        }

        private void Pause()
        {
            Time.timeScale = 0.005f;
            gridCreated = true;
        }

        private void Resume()
        {
            Time.timeScale = 1;
        }

        public void Initialize()
        {
            DefensesModel.OnSelectDefenseClick += Pause;
            Grid.OnGridCreated += Pause;
            InputManager.OnActiveDefenseClick += Pause;
            AdManager.OnInterstitialAdClosed += Pause;

            WinModel.OnWin += Win;

            InputManager.OnEnemyClick += Resume;
            DefensesModel.OnDefenseDeselected += Resume;
            Castle.OnLose += Resume;
        }

        private void Win() => isWon = true;

        public void Tick()
        {

            if (DefensesModel.InDefenseSelectionMode && !IsPaused)
                Pause();
            if (!gridCreated) return;
            if (IsPaused || !Castle.IsAlive || isWon) return;


            if (Time.time - lastDropTime > CurrencyModel.SecondsToDropGold)
            {
                var gold = Mathf.Clamp(CurrencyModel.Gold, 1, CurrencyModel.Gold);
                CurrencyModel.AddGold(Mathf.RoundToInt(CurrencyModel.Gold * CurrencyModel.GoldPercent / 100f));
                lastDropTime = Time.time;
            }
        }
    }
}