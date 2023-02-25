using Apex.AI;

public class HasObservations : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        return Enemies.AIManager.Instance.ObservationCount * 10;
    }
}
