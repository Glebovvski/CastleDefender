using System;
using Models;
using Views;
using Zenject;

namespace ViewModels
{
    public class TutorialViewModel : IInitializable
    {
        private TutorialModel TutorialModel { get; set; }

        public bool IsTutorialComplete => TutorialModel.IsTutorialCompleted;

        [Inject]
        private void Construct(TutorialModel tutorialModel)
        {
            TutorialModel = tutorialModel;
        }

        public void Initialize()
        {
            TutorialModel.OnStepInited += InitTutorialStep;
            TutorialModel.OnTutorialCompleted += EndTutorial;
        }

        public event Action<string, TutorialPlacement> OnStepSet;
        private void InitTutorialStep(string text, TutorialPlacement placement) => OnStepSet?.Invoke(text, placement);

        public event Action OnTutorialStart;
        public void StartTutorial() => OnTutorialStart?.Invoke();

        public event Action OnTutorialClick;
        public void TutorialClick() => OnTutorialClick?.Invoke();

        public event Action OnTutorialEnd;
        public void EndTutorial() => OnTutorialEnd?.Invoke();
    }
}