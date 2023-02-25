using System.Collections;
using Enemies;
using Managers;
using UnityEngine;

namespace Missiles
{
    public class LaunchableMissile : Missile
    {
        public Vector3 Target { get; private set; }
        private Vector3 InitPos { get; set; }
        private Vector3 middle;
        private Transform targetFX;

        public override void OnTriggerEnter(Collider other)
        {
            Explode();
        }

        private void Explode()
        {
            var colliders = Physics.OverlapSphere(this.transform.position, HitRadius, LayerMask.GetMask("Enemy"));
            foreach (var damagable in colliders)
            {
                if (damagable.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.TakeDamage(Damage);
                }
            }
            ReturnToPool();
        }

        public void Launch(Vector3 target)
        {
            targetFX = PoolManager.Instance.GetFromPool<Transform>(PoolObjectType.TargetFX);
            targetFX.position = target;
            targetFX.localScale = new Vector3(HitRadius, targetFX.localScale.y, HitRadius);
            middle = InitPos + (target - InitPos) / 2f + new Vector3(0, 20, 0);
            InitPos = this.transform.position;
            Target = target;
            StartCoroutine(LaunchRoutine());
            StartCoroutine(SelfDestroy());
        }

        private IEnumerator LaunchRoutine()
        {
            float t = 0;
            var time = 1;
            while (t < time)
            {
                t += Time.deltaTime * Speed;
                var position = CalculatePosition(t, Target);
                transform.position = Vector3.Slerp(transform.position, position, t / time);

                yield return null;
            }
        }

        private IEnumerator SelfDestroy()
        {
            yield return new WaitForSeconds(5);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            StopCoroutine(SelfDestroy());
            PoolManager.Instance.ReturnToPool(targetFX.gameObject, PoolObjectType.TargetFX);
            PoolManager.Instance.ReturnToPool(this.gameObject, PoolObjectType.LaunchableMissile);
        }

        private Vector3 CalculatePosition(float t, Vector3 target)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            return uu * InitPos + 2 * u * t * middle + tt * target;

        }
    }
}