using System.Collections;
using Enemies;
using Missiles;
using UnityEngine;

namespace Defendable
{
    public class LaserTowerDefense : ActiveDefense
    {
        [SerializeField] private Transform laserStartPos;
        [SerializeField] private Laser laser;

        [SerializeField] LineRenderer laserRenderer;
        private bool isAttacking = false;
        private Enemy enemyToAttack;

        private bool CanAttack => IsReady && Detection.Enemy != null;
        private bool IsAttackTargetChanged => enemyToAttack != Detection.Enemy && enemyToAttack && Detection.Enemy;

        private void Update()
        {
            if (!isAttacking && CanAttack)
                Attack();
        }

        public void Attack()
        {
            isAttacking = true;
            enemyToAttack = Detection.Enemy;
            StartCoroutine(LaserShot(enemyToAttack));
        }

        IEnumerator LaserShot(Enemy enemy)
        {
            float t = 0;
            float time = 5;
            Vector3 endLaserPos = laser.transform.position;
            laserRenderer.SetPosition(0, laserStartPos.position);

            for (; t < time; t += Time.deltaTime)
            {
                endLaserPos = Vector3.Lerp(laserStartPos.position, enemy.transform.position, t / time);
                laserRenderer.SetPosition(1, endLaserPos);
                yield return null;
            }

            while (enemy.IsAlive && Detection.IsEnemyInRange(enemy))
            {
                UpdateLaser(enemy);
                yield return new WaitForFixedUpdate();
            }
            ResetLaser();
        }

        private void ResetLaser()
        {
            if (IsReady) LastAttackTime = Time.time;
            isAttacking = false;
            laserRenderer.SetPositions(new Vector3[2] { laserStartPos.position, laserStartPos.position });
        }

        private void UpdateLaser(Enemy enemy)
        {
            laserRenderer.SetPosition(0, laserStartPos.position);
            laserRenderer.SetPosition(1, enemy.transform.position);
            var Damage = laser.Damage * Time.deltaTime;
            enemy.TakeDamage(Damage);
        }
    }
}