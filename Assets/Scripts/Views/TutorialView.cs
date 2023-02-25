using UnityEngine;
using System;
using Zenject;
using ViewModels;

namespace Views
{
    public class TutorialView : MonoBehaviour
    {
        private TutorialViewModel ViewModel { get; set; }
        private MenuViewModel MenuViewModel { get; set; }

        [SerializeField] private GameObject panel;
        [SerializeField] private PositionTextDictionary texts;

        private bool IsClickedOnTutorial => Input.GetMouseButtonDown(0) || IsTouched;

        private bool IsTouched
        {
            get
            {
                if (Input.touchCount == 0) return false;
                var touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Began;
            }
        }

        [Inject]
        private void Construct(TutorialViewModel tutorialViewModel, MenuViewModel menuViewModel)
        {
            ViewModel = tutorialViewModel;
            MenuViewModel = menuViewModel;
        }

        private void Start()
        {
            MenuViewModel.OnClose += CheckTutorialState;
            ViewModel.OnTutorialStart += Open;
            ViewModel.OnStepSet += StartTutorialStep;
            ViewModel.OnTutorialEnd += Close;
        }

        private void CheckTutorialState()
        {
            if (ViewModel.IsTutorialComplete)
                Close();
            else
                Open();
        }

        private void StartTutorialStep(string text, TutorialPlacement placement)
        {
            CloseAllTexts();

            var textField = texts[placement];
            textField.SetText(text);
            textField.Show();
        }

        private void CloseAllTexts()
        {
            foreach (var text in texts)
            {
                text.Value.Hide();
            }
        }

        private void Open()
        {
            panel.SetActive(true);
        }

        private void Close()
        {
            panel.SetActive(false);
        }

        private void Update()
        {
            if (IsClickedOnTutorial)
            {
                ViewModel.TutorialClick();
            }
        }
    }

    [Serializable]
    public class PositionTextDictionary : SerializableDictionary<TutorialPlacement, TutorialTextComponent>
    {

    }

    [Serializable]
    public enum TutorialPlacement
    {
        UpperLeft = 0,
        LowerCentre = 1,
        UpperCentre = 2,
        UpperRight = 3,
    }
}