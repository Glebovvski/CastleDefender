using System;
using Managers;
using Zenject;

namespace ViewModels
{
    public class MenuViewModel
    {
        private DefensesViewModel DefensesViewModel { get; set; }
        private IAPManager IAPManager { get; set; }
        private PlaneManager PlaneManager { get; set; }

        public bool IsMenuOpen { get; private set; } = true;

        [Inject]
        private void Construct(DefensesViewModel defensesViewModel, IAPManager iAPManager, PlaneManager planeManager)
        {
            DefensesViewModel = defensesViewModel;
            IAPManager = iAPManager;
            PlaneManager = planeManager;
        }

        public event Action OnOpen;
        public void OpenMenu()
        {
            IsMenuOpen = true;
            DefensesViewModel.Close();
            OnOpen?.Invoke();
        }

        public event Action OnClose;
        public void Close()
        {
            IsMenuOpen = false;
            // if (PlaneManager.GridCreated)
            DefensesViewModel.Open();
            OnClose?.Invoke();
        }

        internal void BuyCoins() => IAPManager.BuyConsumable(IAPManager.coins);
        internal void Donation() => IAPManager.BuyConsumable(IAPManager.donation);
        internal void NoAds() => IAPManager.BuyNonConsumable();
    }
}