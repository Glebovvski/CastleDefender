using Defendable;
using Enemies;
using Grid;
using Managers;
using UnityEngine;
using ViewModels;
using Zenject;

namespace Views
{
    public class DefensesView : MonoBehaviour
    {
        private DefensesViewModel ViewModel { get; set; }
        private AudioManager AudioManager { get; set; }
        private CastleDefense Castle { get; set; }
        
        [SerializeField] private GameObject defensesPanel;
        [SerializeField] private GameObject cancelBtn;

        [Inject]
        private void Construct(DefensesViewModel vm, AudioManager audioManager, CastleDefense castle)
        {
            ViewModel = vm;
            AudioManager = audioManager;
            Castle = castle;
        }

        private void Start()
        {
            ViewModel.OnDefenseSelected += DefenseSelected;
            ViewModel.OnOpen += Show;
            ViewModel.OnClose += Hide;
        }

        public void Show()
        {
            defensesPanel.SetActive(true);
        }
        public void Hide()
        {
            defensesPanel.SetActive(false);
        }
        public void CancelSelection()
        {
            AudioManager.PlayUI();
            ViewModel.DeselectDefense();
            ToggleCancelBtn(false);
        }

        private void DefenseSelected()
        {
            AudioManager.PlayUI();
            ToggleCancelBtn(true);
        }

        public void ToggleCancelBtn(bool active) => cancelBtn.SetActive(active);

        public void ResetCamera() => ViewModel.ResetCamera();

        //TEST

        public void Win()
        {
            AIManager.Instance.DestroyAll();
        }

        public void Lose()
        {
            Castle.TakeDamage(100000);
        }

        //END TEST


        private void OnDestroy()
        {
            ViewModel.OnDefenseSelected -= DefenseSelected;
            ViewModel.OnOpen -= Show;
            ViewModel.OnClose -= Hide;
        }
    }
}