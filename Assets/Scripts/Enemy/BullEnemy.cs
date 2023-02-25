namespace Enemies
{
    public class BullEnemy : Enemy
    {
        public override void StartAttack(out bool isReady)
        {
            base.StartAttack(out isReady);
            if (isReady)
                animationController.Attack();
        }

        public void Attack()
        {
            if (AttackTarget == null)
                return;

            AttackTarget.Defense.TakeDamage(AttackForce);
        }
    }
}