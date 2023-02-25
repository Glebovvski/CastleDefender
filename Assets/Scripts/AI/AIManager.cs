using System;
using System.Collections.Generic;
using System.Linq;
using Defendable;
using Grid;
using Managers;
using Models;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class AIManager : MonoBehaviour
    {
        public static AIManager Instance;

        private CurrencyModel CurrencyModel { get; set; }
        private CastleDefense Castle { get; set; }
        private GameGrid Grid { get; set; }
        private GameControlModel GameModel { get; set; }
        private StatManager StatManager { get; set; }
        private InputManager InputManager { get; set; }

        [SerializeField] private PlaneManager planeManager;

        [Header("Battle settings")]
        [Range(1, 25)]
        [SerializeField] private int maxWaves;
        [Range(0, 25)]
        [SerializeField] private int maxBaseEnemies;
        [Range(0, 5)]
        [SerializeField] private int maxBullEnemies;
        [Range(0, 5)]
        [SerializeField] private int maxHealerEnemies;
        [Range(0, 5)]
        [SerializeField] private int maxSpyEnemies;
        [Range(0, 10)]
        [SerializeField] private int maxKamikazeEnemies;
        [Range(0, 5)]
        [SerializeField] private int maxFlamerEnemies;

        [SerializeField] private Transform[] spawnPoints;

        public int ObservationCount => Observations.Count;

        public event Action<int> OnEnemyDestroyed;

        private List<Observation> Observations = new List<Observation>();
        private List<Enemy> Enemies = new List<Enemy>();

        private Dictionary<PoolObjectType, int> enemyCoefs = new Dictionary<PoolObjectType, int>();

        private int Wave = 0;

        [Inject]
        private void Construct(CurrencyModel currencyModel, GameGrid grid, CastleDefense castle, GameControlModel gameModel, StatManager statManager, InputManager inputManager)
        {
            CurrencyModel = currencyModel;
            Grid = grid;
            Castle = castle;
            GameModel = gameModel;
            StatManager = statManager;
            InputManager = inputManager;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Wave = 0;
            Castle.OnLose += ReturnAllEnemiesToPool;
            GameModel.OnRestart += ResetAIManager;
            planeManager.OnGridSet += SpawnEnemies;

            enemyCoefs.Add(PoolObjectType.Enemy, maxBaseEnemies);
            enemyCoefs.Add(PoolObjectType.SpyEnemy, maxSpyEnemies);
            enemyCoefs.Add(PoolObjectType.FlamerEnemy, maxFlamerEnemies);
            enemyCoefs.Add(PoolObjectType.BullEnemy, maxBullEnemies);
            enemyCoefs.Add(PoolObjectType.HealerEnemy, maxHealerEnemies);
            enemyCoefs.Add(PoolObjectType.KamikazeEnemy, maxKamikazeEnemies);
        }

        private void SpawnEnemies()
        {
            if (Wave > maxWaves)
                return;
            int maxEnemies = enemyCoefs.Select(x => x.Value).Sum();
            if (maxEnemies - Enemies.Count > maxBaseEnemies / 2 && Wave > 0)
                return;

            foreach (var enemy in enemyCoefs)
            {
                var spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)];
                var enemiesOnMap = Enemies.Where(x => x.Type == enemy.Key).Count();
                if (enemiesOnMap < enemy.Value)
                {
                    int enemiesToSpawn = UnityEngine.Random.Range(0, enemy.Value);
                    for (int i = 0; i < enemiesToSpawn; i++)
                        RegisterEnemy(enemy.Key, spawnPoint);
                }
            }

            if (Enemies.Where(x => x.Type != PoolObjectType.SpyEnemy || x.Type != PoolObjectType.HealerEnemy).Count() == 0)
            {
                SpawnEnemies();
                Wave--;
            }
            Wave++;
        }

        private void ResetAIManager()
        {
            Wave = 0;
            maxWaves = UnityEngine.Random.Range(1, 25);
            SpawnEnemies();
        }

        public void AddObservation(Observation observation)
        {
            if (Observations.Any(x => x.Equals(observation))) return;

            Observations.Add(observation);
        }

        public void AddObservation(List<Observation> observations)
        {
            foreach (var observation in observations)
            {
                AddObservation(observation);
            }
        }

        public void RemoveObservation(Observation observation)
        {
            if (observation == null) return;
            if (Observations.Any(x => x.Equals(observation)))
                Observations.Remove(observation);
        }

        public Observation GetClosest(IEnemy enemy) => GetClosest(enemy, Observations);

        public Enemy GetClosestEnemyByType(IEnemy enemy, EnemyType type) => Enemies.Where(x => (x.Position - enemy.Position).sqrMagnitude < enemy.ScanRange * enemy.ScanRange && x.EnemyType == type).FirstOrDefault();

        public Observation GetClosestObservationByType(IEnemy enemy, DefenseType type)
        {
            var observations = Observations.Where(x => x.Defense.Type == type).ToList();
            if (observations.Count == 0)
                return null;

            return GetClosest(enemy, observations);
        }

        public List<Observation> GetObservationsOfType(DefenseType type) => Observations.Where(x => x.Defense.Type == type).ToList();

        public List<Observation> GetActiveDefenses(DefenseType except = DefenseType.None) => Observations.Where(x => x.Defense.IsActiveDefense && x.Defense.Type != except).ToList();


        public event Action<Enemy, PoolObjectType> OnEnemySpawn;

        public Enemy RegisterEnemy(PoolObjectType enemyType, Transform parent)
        {
            var enemy = PoolManager.Instance.GetFromPool<Enemy>(enemyType);
            enemy.gameObject.SetActive(false);
            enemy.Init(InputManager);
            enemy.transform.position = parent.position;
            enemy.OnDeath += GetGoldFromEnemy;
            Enemies.Add(enemy);
            enemy.gameObject.SetActive(true);
            OnEnemySpawn?.Invoke(enemy, enemyType);
            return enemy;
        }

        public event Action<Enemy, PoolObjectType> OnEnemyKilled;
        public void UnregisterEnemy(Enemy enemy)
        {
            OnEnemyKilled?.Invoke(enemy, enemy.Type);
            Enemies.Remove(enemy);
            PoolManager.Instance.ReturnToPool(enemy.GameObject, enemy.Type);
            StatManager.AddEnemiesKilled();
            SpawnEnemies();
            OnEnemyDestroyed?.Invoke(Enemies.Count);
        }

        public IEnumerable<Enemy> GetEnemiesAttackingObservation(Observation observation) => Enemies.Where(x => x.AttackTarget == observation);

        public IEnumerable<Enemy> GetClosestEnemiesWithSameTarget(Enemy enemy) => Enemies.Where(x => (x.Position - enemy.Position).sqrMagnitude < enemy.ScanRange * enemy.ScanRange && x.AttackTarget == enemy.AttackTarget);

        public List<Vector3> GridCorners() => Grid.Corners();
        public Vector3 GridCentre() => Grid.Centre;

        private void GetGoldFromEnemy(Enemy enemy)
        {
            CurrencyModel.AddGold(enemy.GoldToDrop);
        }

        public List<Enemy> GetEnemiesInRangeWithHealthLowerThan(Enemy enemy, float percent) => Enemies.Where(x => (x.Position - enemy.Position).sqrMagnitude < enemy.ScanRange * enemy.ScanRange
        && (x.CurrentHealth / x.Health) <= percent && x.CurrentHealth > 0)
        .ToList();

        public Enemy GetClosest(Enemy enemy, List<Enemy> selectedEnemies) => selectedEnemies.OrderBy(x => (x.Position - enemy.Position).sqrMagnitude).FirstOrDefault();

        public float GetDistanceToGrid(Vector3 enemyPosition) => (Grid.transform.position - enemyPosition).sqrMagnitude;

        private Observation GetClosest(IEnemy enemy, List<Observation> observations)
        {
            Vector3 position = enemy.GameObject.transform.position;
            float distance = 1000;
            Observation closest = null;
            for (int i = 0; i < observations.Count; i++)
            {
                var currDistance = (observations[i].Position - position).sqrMagnitude;
                if (currDistance < distance)
                {
                    distance = currDistance;
                    closest = observations[i];
                }
            }
            return closest;
        }

        private void ReturnAllEnemiesToPool()
        {
            foreach (var enemy in Enemies)
            {
                enemy.Death();
                PoolManager.Instance.ReturnToPool(enemy.GameObject, enemy.Type);
            }
            Enemies.Clear();
        }

        private void OnDestroy()
        {
            Castle.OnLose -= ReturnAllEnemiesToPool;
            GameModel.OnRestart -= ResetAIManager;
            planeManager.OnGridSet -= SpawnEnemies;
        }
    }
}