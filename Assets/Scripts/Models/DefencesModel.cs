using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Models
{
    public class DefensesModel : IInitializable
    {
        private GameControlModel GameControlModel { get; set; }

        private List<ScriptableDefense> defenses = new List<ScriptableDefense>();
        public List<ScriptableDefense> List => defenses;
        public bool InDefenseSelectionMode { get; private set; } = false;

        [Inject]
        private void Construct(GameControlModel gameControlModel)
        {
            GameControlModel = gameControlModel;
        }

        public void Initialize()
        {
            GetDefensesInfo();
            GameControlModel.OnRestart += OverrideInDefenseSelectionMode;
        }

        private void OverrideInDefenseSelectionMode()
        {
            InDefenseSelectionMode = true;
        }

        private void GetDefensesInfo()
        {
            defenses = Resources.LoadAll<ScriptableDefense>("SO/Defense").ToList();
        }

        public event Action<ScriptableDefense> OnDefenseSelected;
        public event Action OnSelectDefenseClick;
        public void DefenseSelected(ScriptableDefense info)
        {
            InDefenseSelectionMode = true;
            OnDefenseSelected?.Invoke(info);
            OnSelectDefenseClick?.Invoke();
        }

        public event Action OnDefenseDeselected;
        public void DefenseDeselected()
        {
            InDefenseSelectionMode = false;
            OnDefenseDeselected?.Invoke();
        }

    }
}