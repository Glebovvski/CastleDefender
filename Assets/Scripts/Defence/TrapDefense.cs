using System.Collections;
using Enemies;
using UnityEngine;
using UnityEngine.AI;

namespace Defendable
{
    public class TrapDefense : Defense
    {
        private const string openTrapKey = "open";
        private const string closeTrapKey = "close";
        [SerializeField] private Animator animator;
        private float lastShotTime;

        protected override bool IsReady { get => Time.time - lastShotTime > ReloadTime; }

        private NavMeshSurface surface;

        public override void OnEnable()
        {
            base.OnEnable();
            OnDefenseSet += UpdateNavMesh;
        }

        private void UpdateNavMesh()
        {
            transform.parent.TryGetComponent<NavMeshSurface>(out surface);
            if (surface)
            {
                surface.BuildNavMesh();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                if (IsReady)
                    StartCoroutine(SetTrap(enemy));
            }
        }

        IEnumerator SetTrap(Enemy enemy)
        {
            animator.SetTrigger(openTrapKey);
            enemy.TakeDamage(AttackForce);
            yield return new WaitForSeconds(1f);
            animator.SetTrigger(closeTrapKey);
            lastShotTime = Time.time;

        }

        private void OnDisable()
        {
            OnDefenseSet -= UpdateNavMesh;
        }
    }
}