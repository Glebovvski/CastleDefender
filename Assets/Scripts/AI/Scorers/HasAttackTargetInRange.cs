using AI;
using Apex.AI;

public class HasAttackTargetInRange : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;
        var attackTarget = enemy.AttackTarget;
        if (attackTarget == null) return 0;
        if (!attackTarget.IsAlive) return 0;
        if (!enemy.IsAttackTargetInRange) return 0;

        return 100;
    }
}
