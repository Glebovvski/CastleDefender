using UnityEngine;
using UnityEngine.AI;

public class AnimationController
{
    private const string SpeedParam = "Speed";
    private const string AttackTrigger = "Attack";
    private const string DeathTrigger = "Death";

    private Animator Animator { get; set; }
    private NavMeshAgent NavMeshAgent { get; set; }
    private DamageReceiver DamageReceiver { get; set; }

    public AnimationController(Animator animator, NavMeshAgent navMeshAgent, DamageReceiver damageReceiver)
    {
        Animator = animator;
        NavMeshAgent = navMeshAgent;
        DamageReceiver = damageReceiver;
        DamageReceiver.OnDeath += PlayDeath;
    }

    public void UpdateState()
    {
        Animator.SetFloat(SpeedParam, NavMeshAgent.velocity.sqrMagnitude);
    }

    public void Attack()
    {
        Animator.SetTrigger(AttackTrigger);
    }

    private void PlayDeath()
    {
        DamageReceiver.OnDeath -= PlayDeath;
        Animator.SetTrigger(DeathTrigger);
    }
}
