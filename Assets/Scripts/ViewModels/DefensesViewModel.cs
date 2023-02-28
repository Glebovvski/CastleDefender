using System;
using Defendable;
using Managers;
using Models;
using UnityEngine;
using Views;
using Zenject;

namespace ViewModels
{
    public class DefensesViewModel : MonoBehaviour
    {
        private MenuViewModel MenuViewModel { get; set; }

        [SerializeField] private DefenseView prefab;
        [SerializeField] private Transform viewParent;
        private DefensesModel DefensesModel { get; set; }
        private GameControlModel GameModel { get; set; }
        private DiContainer Container { get; set; }

        public event Action OnDefenseSelected;

        [Inject]
        private void Construct(DefensesModel defensesModel, GameControlModel gameControlModel, DiContainer container, MenuViewModel menuViewModel)
        {
            Container = container;
            DefensesModel = defensesModel;
            GameModel = gameControlModel;
            MenuViewModel = menuViewModel;
        }

        private void Start()
        {
            DefensesModel.OnSelectDefenseClick += DefenseSelected;
            GameModel.OnRestart += DefenseSelected;
            MenuViewModel.OnOpen += Close;
            MenuViewModel.OnClose += Open;


            DefenseViewFactory factory = Container.Resolve<DefenseViewFactory>();
            foreach (var defense in DefensesModel.List)
            {
                factory.CreateDefenseView(defense, prefab, viewParent);
            }
        }

        public event Action OnClose;
        public void Close() => OnClose?.Invoke();

        public event Action OnOpen;
        public void Open()
        {
            OnOpen?.Invoke();
        }
        public void DefenseSelected() => OnDefenseSelected?.Invoke();

        public void DeselectDefense()
        {
            DefensesModel.DefenseDeselected();
        }

        public event Action OnResetCameraClick;
        public void ResetCamera()
        {
            OnResetCameraClick?.Invoke();
        }

        private void OnDestroy()
        {
            DefensesModel.OnSelectDefenseClick -= DefenseSelected;
            GameModel.OnRestart -= DefenseSelected;
            MenuViewModel.OnOpen -= Close;
            MenuViewModel.OnClose -= Open;
        }
    }
}