using AI;
using Apex.AI;
using Apex.Serialization;

public class IsHealthBelowAmount : ContextualScorerBase
{
    [ApexSerialization] private float healthThreshold;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        if (enemy.CurrentHealth / enemy.Health <= healthThreshold)
            return 200;
        return 0;
    }
}
