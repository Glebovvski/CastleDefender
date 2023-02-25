using AI;
using Apex.AI;
using Enemies;

public class IsFollowTargetInRange : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;
        if (enemy.FollowTarget == null) return 0;
        if (!enemy.FollowTarget.IsAlive) return 0;
        if (IsNotInHealRange(enemy)) return 0;

        return 100;
    }

    private bool IsNotInHealRange(Enemy enemy)
    {
        var result = (enemy.FollowTarget.Position - enemy.Position).sqrMagnitude > enemy.AttackRange*enemy.AttackRange;
        return result;
    }
}
