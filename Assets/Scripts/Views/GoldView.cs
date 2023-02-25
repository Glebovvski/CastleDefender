using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using ViewModels;

namespace Views
{
    public class GoldView : MonoBehaviour
    {
        private GoldViewModel GoldViewModel { get; set; }

        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI goldPercent;

        [Inject]
        private void Construct(GoldViewModel goldViewModel)
        {
            GoldViewModel = goldViewModel;
        }

        private void Start()
        {
            GoldViewModel.OnGoldChanged += UpdateText;
            goldPercent.text = GoldViewModel.GetGoldPercent();
        }

        private void UpdateText(int value)
        {
            goldText.text = value.ToString();
        }

        private void OnDestroy()
        {
            GoldViewModel.OnGoldChanged -= UpdateText;
        }
    }
}