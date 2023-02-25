using AI;
using Apex.AI;

public class Attack : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        c.Enemy.StartAttack(out bool isReady);
    }
}
