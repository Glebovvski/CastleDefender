using System;
using System.Collections.Generic;
using Defendable;
using UnityEngine;

namespace Enemies
{
    public class FlamerEnemy : Enemy
    {
        [SerializeField] private GameObject fire;
        public List<Defense> defensesInRange = new List<Defense>();

        private bool IsAttacking { get; set; } = false;

        private float startAttackTime = 0;

        public override void StartAttack(out bool isReady)
        {
            base.StartAttack(out isReady);
            if (isReady)
            {
                StartAttack();
                animationController.Attack();
            }
        }

        public void StartAttack()
        {
            transform.LookAt(AttackTarget.Defense.transform);
            startAttackTime = Time.time;
            fire.SetActive(true);
            IsAttacking = true;

        }

        public override void Update()
        {
            base.Update();

            if (!IsAttacking)
                return;

            if (Time.time - startAttackTime > 1)
                StopAttack();

            foreach (var defense in defensesInRange)
            {
                defense.TakeDamage(AttackForce * Time.deltaTime);
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.TryGetComponent<Defense>(out var defense))
            {
                defensesInRange.Add(defense);
            }
        }

        public void StopAttack()
        {
            IsAttacking = false;
            defensesInRange.Clear();
            fire.SetActive(false);
        }
    }
}