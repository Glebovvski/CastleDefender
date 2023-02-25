using Defendable;
using Models;
using UnityEngine;
using Views;
using Zenject;

namespace ViewModels
{
    public class LoseViewModel : MonoBehaviour
    {
        private CastleDefense Castle { get; set; }
        private GameControlModel GameControlModel { get; set; }
        private MenuViewModel MenuViewModel { get; set; }

        [SerializeField] private LoseView view;

        [Inject]
        private void Construct(CastleDefense castle, GameControlModel gameModel, MenuViewModel menuViewModel)
        {
            Castle = castle;
            GameControlModel = gameModel;
            MenuViewModel = menuViewModel;
        }

        private void Start()
        {
            Castle.OnDeath += ShowLoseView;
            view.OnTryAgainClick += TryAgain;
            view.OnMenuClick += Menu;
        }

        private void Menu()
        {
            GameControlModel.Restart();
            view.Close();
            MenuViewModel.OpenMenu();
        }

        private void ShowLoseView()
        {
            view.Open();
        }

        public void TryAgain()
        {
            GameControlModel.Restart();
            view.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Castle.OnDeath -= ShowLoseView;
        }

    }
}