using System.Collections.Generic;
using UnityEngine;
using Enemies;

namespace Defendable
{
    public class DetectionRadius : MonoBehaviour
    {
        [SerializeField] private SphereCollider Radius;
        private List<Enemy> enemiesInRange = new List<Enemy>();
        public Enemy Enemy => AttackTarget && AttackTarget.IsAlive ? AttackTarget : ClosestEnemy();
        public Enemy AttackTarget { get; private set; } = null;

        public void SetAttackRange(int radius) => Radius.radius = radius;

        public bool IsEnemyInRange(Enemy enemy) => enemiesInRange.Contains(enemy);

        public void SetAttackTarget(Enemy enemy) => AttackTarget = enemy;

        private Enemy ClosestEnemy()
        {
            float minDistance = 100;
            Enemy closestEnemy = null;
            foreach (var enemy in enemiesInRange)
            {
                var distance = Vector3.Distance(enemy.transform.position, this.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Enemy>(out var enemy)) return;

            enemiesInRange.Add(enemy);
            enemy.OnDeath += UnregisterEnemy;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Enemy>(out var enemy)) return;

            enemiesInRange.Remove(enemy);
        }

        private void UnregisterEnemy(Enemy enemy)
        {
            enemiesInRange.Remove(enemy);
            enemy.OnDeath -= UnregisterEnemy;
        }
    }
}