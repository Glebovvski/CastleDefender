using AI;
using Apex.AI;
using Enemies;

public class MoveToHealer : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var healer = Enemies.AIManager.Instance.GetClosestEnemyByType(enemy, EnemyType.Healer);
        if (healer && healer.isActiveAndEnabled && enemy.IsNewDestination(healer.Position))
        {
            enemy.FollowTarget = healer;
            enemy.MoveTo(enemy.FollowTarget.Position);
        }
    }
}
