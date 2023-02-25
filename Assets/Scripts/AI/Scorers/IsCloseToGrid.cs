using AI;
using Apex.AI;
using Enemies;

public class IsCloseToGrid : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        return Enemies.AIManager.Instance.GetDistanceToGrid(enemy.Position) < enemy.ScanRange*enemy.ScanRange ? 100 : 0; 
    }
}
