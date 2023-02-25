using System;
using System.Collections.Generic;
using System.Linq;
using Missiles;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class PoolManager : MonoBehaviour
    {
        private DiContainer Container { get; set; }

        public static PoolManager Instance { get; private set; }

        private PlaneManager PlaneManager { get; set; }

        [SerializeField] private List<PoolInfo> poolList;
        [SerializeField] private Type type;

        [Inject]
        private void Construct(PlaneManager planeManager, DiContainer container)
        {
            PlaneManager = planeManager;
            Container = container;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            for (int i = 0; i < poolList.Count; i++)
            {
                FillPool(poolList[i]);
            }
        }

        private void FillPool(PoolInfo info)
        {
            for (int i = 0; i < info.amount; i++)
            {
                GameObject go = null;
                go = Container.InstantiatePrefab(info.prefab, this.transform);
                go.SetActive(false);
                info.pool.Add(go);
            }
        }

        public void ReturnToPool(GameObject go, PoolObjectType type)
        {
            PoolInfo poolInfo = poolList.FirstOrDefault(x => x.type == type);
            go.transform.SetParent(poolInfo.container.transform);
            go.transform.localScale = Vector3.one;
            go.SetActive(false);
            bool IsNotInPool = !poolInfo.pool.Contains(go);
            if (IsNotInPool)
                poolInfo.pool.Add(go);
            go.transform.position = new Vector3(-100, -100, -100);
        }

        public T GetFromPool<T>(PoolObjectType type)
        {
            GameObject go;
            var poolInfo = GetPoolInfoByType(type);
            var pool = poolInfo.pool;
            if (pool.Count == 0)
                go = Instantiate(poolInfo.prefab, poolInfo.container.transform);
            else
            {
                go = pool.FirstOrDefault(x => !x.activeSelf);
                pool.Remove(go);
            }
            go.SetActive(true);
            PlaneManager.AttachChild(go.transform);
            return go.GetComponent<T>();
        }

        private PoolInfo GetPoolInfoByType(PoolObjectType type) => poolList.FirstOrDefault(x => x.type == type);
    }

    [Serializable]
    public class PoolInfo
    {
        public PoolObjectType type;
        public int amount;
        public GameObject prefab;
        public GameObject container;

        [HideInInspector] public List<GameObject> pool = new List<GameObject>();
    }

    public enum PoolObjectType
    {
        None = 0,
        CannonTower = 100,
        WallTower,
        LaserTower,
        CastleTower,
        TrapTower,
        MissileLauncher,
        Enemy = 200,
        SpyEnemy,
        BullEnemy,
        KamikazeEnemy,
        HealerEnemy,
        FlamerEnemy,
        CannonBullet = 300,
        LaunchableMissile,
        DecorTree01 = 1000,
        DecorTree02,
        DecorAppleTree01,
        DecorAppleTree02,
        DecorGrass01,
        DecorGrass02,
        DecorPine01,
        DecorPine02,
        DecorPine03,
        DecorPine04,
        TargetFX = 1100,

    }
}