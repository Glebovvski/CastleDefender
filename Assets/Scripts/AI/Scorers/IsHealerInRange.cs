using AI;
using Apex.AI;

public class IsHealerInRange : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var healer = Enemies.AIManager.Instance.GetClosestEnemyByType(enemy, Enemies.EnemyType.Healer);
        if (healer != null && healer.isActiveAndEnabled)
            return 100;
        else return 0;
    }
}
