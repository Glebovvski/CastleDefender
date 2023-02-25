using AI;
using Apex.AI;

public sealed class HasNoAttackTargetInRange : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;

        if (c.Enemy.AttackTarget == null || !c.Enemy.AttackTarget.IsAlive)
        {
            return 100f;
        }
        if((c.Enemy.AttackTarget.Position - c.Enemy.Position).sqrMagnitude <= c.Enemy.AttackRange * c.Enemy.AttackRange)
            return this.score;

        return 100f;
    }
}
