using System;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Apex.AI.Components;
using Defendable;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using CartoonFX;
using cakeslice;
using Zenject;
using Managers;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, IContextProvider, IEnemy
    {
        private InputManager InputManager { get; set; }

        [SerializeField] private ScriptableEnemy SO;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Animator animator;
        [SerializeField] private CFXR_Effect fx;
        [SerializeField] private Outline outline;
        [SerializeField] private Transform attackTrigger;

        public int Health { get; private set; }
        public float CurrentHealth => DamageReceiver.CurrentHealth;
        public int AttackForce { get; set; }
        public int Speed { get; set; }
        public int GoldToDrop => SO.GoldToDrop;
        public bool IsAlive => !DamageReceiver.IsDead;
        public AIContext Context { get; private set; }
        public GameObject GameObject => this.gameObject;
        public NavMeshAgent NavMeshAgent => navMeshAgent;
        public Observation AttackTarget { get; private set; }
        public Enemy FollowTarget { get; set; }
        public float ScanRange => SO.ScanRange;
        public Vector3 Position => this.gameObject.transform.position;
        public DamageReceiver DamageReceiver;
        public PoolObjectType Type => SO.Type;
        public float AttackRange => SO.AttackRange * transform.parent.localScale.x;
        public float AttackRadius => SO.AttackRadius;
        public EnemyType EnemyType => SO.EnemyType;
        protected bool IsReadyToAttack => Time.time - LastAttackTime > SO.TimeBetweenAttacks;

        public int AttackWallScore => SO.AttackWallScore;
        public int AttackCannonScore => SO.AttackCannonScore;
        public int AttackLaserScore => SO.AttackLaserScore;
        public int AttackCastleScore => SO.AttackCastleScore;
        public int AttackTrapScore => SO.AttackTrapScore;

        public bool IsAttackTargetInRange { get; private set; } = false;

        protected float LastAttackTime { get; set; }

        public event Action<Enemy> OnDeath;

        public Dictionary<DefenseType, int> DefenseTypeToScore = new Dictionary<DefenseType, int>();

        public IAIContext GetContext(Guid aiId) => Context;

        protected AnimationController animationController;

        public virtual void Init(InputManager inputManager)
        {
            InputManager = inputManager;

            GetData();
            DamageReceiver = new DamageReceiver(Health);
            Context = new AIContext(this);
            animationController = new AnimationController(animator, navMeshAgent, DamageReceiver);
            DamageReceiver.OnDeath += Death;
            NavMeshAgent.enabled = true;
            fx.OnFinish += RemoveEnemyFromField;
            fx.gameObject.SetActive(false);

            outline.enabled = false;
            InputManager.OnActiveDefenseClick += OutlineEnemy;
            InputManager.OnEnemyClick += CancelOutline;
            PopulateDictionary();
            attackTrigger.localScale = new Vector3(1, 1, AttackRange);
        }


        private void PopulateDictionary()
        {
            DefenseTypeToScore.Clear();
            DefenseTypeToScore.Add(DefenseType.Castle, AttackCastleScore);
            DefenseTypeToScore.Add(DefenseType.Cannon, AttackCannonScore);
            DefenseTypeToScore.Add(DefenseType.Laser, AttackLaserScore);
            DefenseTypeToScore.Add(DefenseType.Wall, AttackWallScore);
            DefenseTypeToScore.Add(DefenseType.Trap, AttackTrapScore);

            DefenseTypeToScore = DefenseTypeToScore.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        public virtual void Update()
        {
            if (AttackTarget != null)
            {
                var targetPos = new Vector3(AttackTarget.Defense.transform.position.x, this.transform.position.y, AttackTarget.Defense.transform.position.z);
                transform.LookAt(targetPos);
            }
            animationController.UpdateState();
        }

        public void SetAttackTarget(Observation observation)
        {
            AttackTarget = observation;
            AttackTarget.Defense.OnDeath += ResetTarget;
            observation.SetAsAttackTarget();
        }

        public void GetData()
        {
            Health = SO.Health;
            AttackForce = SO.AttackForce;
            Speed = SO.Speed;
            navMeshAgent.speed = SO.Speed;
        }

        public bool IsNewDestination(Vector3 destination) => Vector3.Distance(NavMeshAgent.destination, destination) > 0.1f;

        public void MoveTo(Vector3 destination)
        {
            if (NavMeshAgent.enabled)
                NavMeshAgent.destination = destination;
        }

        public void TakeDamage(float value) => DamageReceiver.TakeDamage(value);

        public NavMeshPath GetCalculatedPath(Observation observation)
        {
            var navMeshPath = new NavMeshPath();
            NavMeshAgent.CalculatePath(observation.Position, navMeshPath);
            return navMeshPath;
        }

        public virtual void StartAttack(out bool isReady)
        {
            isReady = IsReadyToAttack;
            if (isReady)
                LastAttackTime = Time.time;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            DamageReceiver.OnCollision(other);
            CheckAttackTarget(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (AttackTarget == null) return;

            if (other.TryGetComponent<Defense>(out var defense))
            {
                if (AttackTarget.Defense == defense)
                    IsAttackTargetInRange = false;
            }
        }

        private void CheckAttackTarget(Collider other)
        {
            if (AttackTarget == null) return;

            if (other.TryGetComponent<Defense>(out var defense))
            {
                if (AttackTarget.Defense == defense)
                {
                    IsAttackTargetInRange = true;
                }
            }
        }

        public void Death()
        {
            IsAttackTargetInRange = false;
            AttackTarget = null;
            DamageReceiver.OnDeath -= Death;
            fx.gameObject.SetActive(true);
        }

        private void RemoveEnemyFromField()
        {
            OnDeath?.Invoke(this);
            fx.OnFinish -= RemoveEnemyFromField;
            AIManager.Instance.UnregisterEnemy(this);
        }

        private void ResetTarget()
        {
            AIManager.Instance.RemoveObservation(AttackTarget);
            if (AttackTarget == null) return;

            AttackTarget.Defense.OnDeath -= ResetTarget;
            AttackTarget = null;
        }

        private void OutlineEnemy() => outline.enabled = true;

        private void CancelOutline() => outline.enabled = false;
    }

    public enum EnemyType
    {
        Mono = 0, //for big enemies or spies
        Team = 1, //for small mobs
        Kamikaze = 2,
        Healer = 3,
        Any = 4
    }
}