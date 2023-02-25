using AI;
using Apex.AI;
using Enemies;

public class IsScanFinished : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = (SpyEnemy)c.Enemy;

        return enemy.IsScanFinished ? 100 : 0;
    }
}