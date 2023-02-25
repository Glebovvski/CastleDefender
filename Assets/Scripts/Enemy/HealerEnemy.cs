using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class HealerEnemy : Enemy
    {
        [SerializeField] private ParticleSystem healFx;
        private bool isHealFxPlaying = false;

        public override void Init(InputManager inputManager)
        {
            base.Init(inputManager);
            healFx.gameObject.transform.localScale = new Vector3(AttackRange, 1, AttackRange);
        }

        public void Heal()
        {
            var colliders = Physics.OverlapSphere(Position, AttackRadius, LayerMask.GetMask("Enemy"));
            if (!isHealFxPlaying)
                HealFX();
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Enemy>(out var enemy))
                {
                    var healValue = Mathf.Clamp(AttackForce, 0, enemy.Health);
                    enemy.TakeDamage(-healValue);
                }
            }
        }

        public override void StartAttack(out bool isReady)
        {
            base.StartAttack(out isReady);
            if (isReady)
                Heal();
        }

        public void HealFX()
        {
            StartCoroutine(PlayHeal());
        }

        IEnumerator PlayHeal()
        {
            isHealFxPlaying = true;
            healFx.Play();
            for (float t = 0; t < 1;)
            {
                t += Time.deltaTime;
                yield return null;
            }
            healFx.Stop();
            isHealFxPlaying = false;
        }
    }
}