using System;
using System.Linq;
using CartoonFX;
using Grid;
using Managers;
using UnityEngine;
using Zenject;

namespace Defendable
{
    public abstract class Defense : MonoBehaviour
    {
        private StatManager StatManager { get; set; }

        [SerializeField] protected CFXR_Effect destroyFX;
        [SerializeField] protected GameObject defenseMesh;
        [SerializeField] protected HealthBarController healthBarController;

        [field: SerializeField] public ScriptableDefense SO { get; set; }

        public int Health { get; private set; }
        public float CurrentHealth => DamageReceiver.CurrentHealth;
        public bool IsAlive => CurrentHealth > 0;
        public bool IsActiveDefense { get; private set; }
        protected int AttackRange { get; private set; }
        protected int AttackForce { get; private set; }
        public Vector2Int Size { get; private set; }
        protected int ReloadTime { get; set; }
        protected abstract bool IsReady { get; }
        protected Time ReloadStart { get; private set; }
        public Predicate<GridCell> ConditionToPlace => SO.GetCondition();
        public Predicate<GridCell[]> CanFit = (GridCell[] cells) => cells.All(x => x.IsUpper) || cells.All(x => !x.IsUpper);
        public bool IsActionAvailable() => IsActiveDefense && IsReady;
        public Vector2Int GetSize() => SO.Size;
        public DefenseType Type => SO.Type;
        public BoxCollider Collider { get; private set; }
        protected DamageReceiver DamageReceiver;
        public event Action OnDefenseSet;

        public virtual event Action OnDeath;
        public virtual event Action<Defense> OnDefenseDestroy;

        [Inject]
        private void Construct(StatManager statManager)
        {
            StatManager = statManager;
        }

        public virtual void Init(ScriptableDefense so)
        {
            SO = so;
            IsActiveDefense = SO.IsActiveDefense;
            AttackRange = SO.AttackRange;
            AttackForce = SO.AttackForce;
            Health = SO.Health;
            ReloadTime = SO.ReloadTime;
            Size = SO.Size;
            DamageReceiver = new DamageReceiver(Health);
            DamageReceiver.OnDeath += Death;
            DamageReceiver.OnTakeDamage += UpdateHealthBar;
            destroyFX.OnFinish += ReturnToPool;
            destroyFX.gameObject.SetActive(false);
            defenseMesh.SetActive(true);
            healthBarController.Init();
        }

        protected void UpdateHealthBar()
        {
            healthBarController.UpdateHealth(CurrentHealth / Health);
        }

        public virtual void OnEnable()
        {
            destroyFX.gameObject.SetActive(false);
            defenseMesh.SetActive(true);
        }

        protected void Awake()
        {
            Collider = GetComponent<BoxCollider>();
        }
        public void TakeDamage(float value) => DamageReceiver.TakeDamage(value);

        protected virtual void Death()
        {
            OnDefenseDestroy?.Invoke(this);
            defenseMesh.SetActive(false);
            destroyFX.gameObject.SetActive(true);
        }

        public PoolObjectType DefenseTypeToPoolType(DefenseType type)
        {
            switch (type)
            {
                case DefenseType.Castle:
                    return PoolObjectType.CastleTower;
                case DefenseType.Wall:
                    return PoolObjectType.WallTower;
                case DefenseType.Cannon:
                    return PoolObjectType.CannonTower;
                case DefenseType.Laser:
                    return PoolObjectType.LaserTower;
                case DefenseType.Trap:
                    return PoolObjectType.TrapTower;
                case DefenseType.MissileLauncher:
                    return PoolObjectType.MissileLauncher;
                default:
                    return PoolObjectType.None;
            }
        }

        protected virtual void ReturnToPool()
        {
            DamageReceiver.OnDeath -= OnDeath;
            PoolManager.Instance.ReturnToPool(this.gameObject, DefenseTypeToPoolType(Type));
            OnDeath?.Invoke();
        }

        public void DefenseSet() => OnDefenseSet?.Invoke();

        internal void SetAsTargetToAttack()
        {
            healthBarController.UpdateHealth(CurrentHealth / Health);
        }
    }
}