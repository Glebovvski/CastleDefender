using System;
using Defendable;
using Enemies;
using Grid;
using Models;
using UnityEngine;
using Zenject;

public class InputManager : MonoBehaviour
{
    private DefensesModel DefensesModel { get; set; }
    private GameGrid Grid { get; set; }
    private LayerMask GridLayer;
    private LayerMask DefenseLayer;
    private LayerMask EnemyLayer;

    public event Action OnActiveDefenseClick;
    public event Action OnEnemyClick;


    private ActiveDefense _selectedDefense;
    private ActiveDefense SelectedDefense
    {
        get => _selectedDefense;
        set
        {
            if (_selectedDefense) _selectedDefense.SelectDefense(false); //set outline off for previous defense
            _selectedDefense = value;
            if (_selectedDefense)
            {
                _selectedDefense.SelectDefense(true);
                OnActiveDefenseClick?.Invoke();
            }
        }
    }

    [Inject]
    private void Construct(GameGrid gameGrid, DefensesModel defensesModel)
    {
        DefensesModel = defensesModel;
        Grid = gameGrid;
    }

    private void Start()
    {
        GridLayer = LayerMask.GetMask("Grid");
        DefenseLayer = LayerMask.GetMask("Defense");
        EnemyLayer = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            var cell = GetObjectOnScene<GridCell>(GridLayer);
            if (!TrySpawnOnCell(cell))
            {
                if (DefensesModel.InDefenseSelectionMode) return;

                var defense = GetObjectOnScene<ActiveDefense>(DefenseLayer);
                if ((!defense || !defense.IsActiveDefense) && !SelectedDefense) return;
                if (SelectedDefense != defense && defense)
                {
                    SelectedDefense = defense;
                    return;
                }

                if (SelectedDefense)
                {
                    var enemy = GetObjectOnScene<Enemy>(EnemyLayer);
                    if (!enemy || !enemy.IsAlive || !SelectedDefense || !SelectedDefense.IsEnemyInRange(enemy)) return;
                    SelectedDefense.SetAttackTarget(enemy);
                    OnEnemyClick?.Invoke();
                    SelectedDefense = null;
                }
            }
        }

        // if (!cell || !cell.IsSelected) return;
#elif PLATFORM_IOS || PLATFORM_ANDROID

        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            var cell = GetObjectOnScene<GridCell>(GridLayer);
            if (!TrySpawnOnCell(cell))
            {
                if (DefensesModel.InDefenseSelectionMode) return;

                var defense = GetObjectOnScene<ActiveDefense>(DefenseLayer);
                if ((!defense || !defense.IsActiveDefense) && !SelectedDefense) return;
                if (SelectedDefense != defense && defense)
                {
                    SelectedDefense = defense;
                    return;
                }

                if (SelectedDefense)
                {
                    var enemy = GetObjectOnScene<Enemy>(EnemyLayer);
                    if (!enemy || !enemy.IsAlive || !SelectedDefense || !SelectedDefense.IsEnemyInRange(enemy)) return;
                    SelectedDefense.SetAttackTarget(enemy);
                    OnEnemyClick?.Invoke();
                    SelectedDefense = null;
                }
            }
        }
#endif

    }

    private bool TrySpawnOnCell(GridCell cell)
    {
        if (!cell || !cell.IsSelected)
            return false;
        Grid.SpawnDefence(cell);
        return true;
    }

    private T GetObjectOnScene<T>(LayerMask layer) where T : MonoBehaviour
    {
        Ray ray;
#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#elif UNITY_IOS || PLATFORM_IOS || PLATFORM_ANDROID
        ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif
        if (Physics.Raycast(ray, out var raycastHit, 1000f, layer))
        {
            return raycastHit.transform.GetComponent<T>();
        }
        return null;
    }
}
