using AI;
using Apex.AI;
using Apex.Serialization;

public class IsActiveDefensesInSight : ContextualScorerBase
{
    [ApexSerialization] private bool not;
    [ApexSerialization] private DefenseType except;
    [ApexSerialization] private int ExceptTypeThreshold;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var exceptDefenses = Enemies.AIManager.Instance.GetObservationsOfType(except);
        var activeDefenses = Enemies.AIManager.Instance.GetActiveDefenses(except);

        if (exceptDefenses.Count < ExceptTypeThreshold)
        {
            float score = activeDefenses.Count * 100;
            return not ? -score : score;
        }
        else if (exceptDefenses.Count == 0 && activeDefenses.Count == 0)
            return not ? 100 : -100;
        else return not ? exceptDefenses.Count * 100 : 0;
    }
}
