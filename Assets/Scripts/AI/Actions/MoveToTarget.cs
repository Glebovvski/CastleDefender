using AI;
using Apex.AI;

public class MoveToTarget : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;

        if (c.Enemy.AttackTarget != null && c.Enemy.AttackTarget.IsAlive)
        {
            c.Enemy.MoveTo(c.Enemy.AttackTarget.Position);
        }
    }
}