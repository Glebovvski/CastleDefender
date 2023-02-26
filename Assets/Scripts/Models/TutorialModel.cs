using System;
using Grid;
using Managers;
using UnityEngine;
using ViewModels;
using Views;
using Zenject;

namespace Models
{
    public class TutorialModel : IInitializable
    {
        private PlaneManager PlaneManager { get; set; }
        private MenuViewModel MenuViewModel { get; set; }
        private TutorialViewModel TutorialViewModel { get; set; }
        private GameGrid Grid { get; set; }

        private const string isTutorialCompletedKey = "IsTutorialCompleted";

        private bool stepThreeInited = false;

        private bool isTutorialCompleted = false;
        public bool IsTutorialCompleted
        {
            get => PlayerPrefs.GetInt(isTutorialCompletedKey, 0) == 1;
            private set
            {
                isTutorialCompleted = value;
                PlayerPrefs.SetInt(isTutorialCompletedKey, isTutorialCompleted ? 1 : 0);
            }
        }

        private int simpleTutorialStepIndex = 0;
        private Action[] simpleTutorialSteps;

        [Inject]
        private void Construct(PlaneManager planeManager, MenuViewModel menuViewModel, TutorialViewModel tutorialViewModel, GameGrid grid)
        {
            PlaneManager = planeManager;
            MenuViewModel = menuViewModel;
            TutorialViewModel = tutorialViewModel;
            Grid = grid;
        }

        public void Initialize()
        {
            if (IsTutorialCompleted)
            {
                CompleteTutorial();
                return;
            }

            StartTutorial();

            PlaneManager.OnGridSet += InitStepThree;

            simpleTutorialStepIndex = 0;
            simpleTutorialSteps = new Action[]
            {
                InitStepFour,
                InitStepFive,
                InitStepSix
            };

            TutorialViewModel.OnTutorialClick += InitSimpleStep;
        }

        private void StartTutorial()
        {
            TutorialViewModel.StartTutorial();
        }

        public event Action<string, TutorialPlacement> OnStepInited;
        public void InitStepThree()
        {
            if (stepThreeInited) return;
            if (IsTutorialCompleted) return;
            OnStepInited?.Invoke("To set your defense select Tower from the list and put it on available cell", TutorialPlacement.LowerCentre);
            stepThreeInited = true;
        }
        public void InitStepFour()
        {
            if (IsTutorialCompleted) return;
            OnStepInited?.Invoke("Your current money count is displayed here", TutorialPlacement.UpperLeft);
        }
        public void InitStepFive()
        {
            if (IsTutorialCompleted) return;
            OnStepInited?.Invoke("You can get more money by destroying enemies that come.\nAlso money generate over time with this rate", TutorialPlacement.UpperLeft);
        }
        public void InitStepSix()
        {
            if (IsTutorialCompleted) return;
            OnStepInited?.Invoke("When you done start the game by clicking cross", TutorialPlacement.UpperRight);
        }
        public void InitSimpleStep()
        {
            if (!stepThreeInited) return;

            if (IsTutorialCompleted) return;

            if (simpleTutorialStepIndex >= simpleTutorialSteps.Length)
            {
                CompleteTutorial();
                return;
            }
            simpleTutorialSteps[simpleTutorialStepIndex].Invoke();
            simpleTutorialStepIndex++;
        }

        public event Action OnTutorialCompleted;
        public void CompleteTutorial()
        {
            IsTutorialCompleted = true;
            OnTutorialCompleted?.Invoke();
        }
    }
}