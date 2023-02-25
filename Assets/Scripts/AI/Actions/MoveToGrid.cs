using Apex.AI;

namespace AI
{
    public class MoveToGrid : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var position = Enemies.AIManager.Instance.GridCentre();
            if (c.Enemy.AttackTarget == null && c.Enemy.FollowTarget == null && c.Enemy.IsNewDestination(position))
                c.Enemy.MoveTo(position);
        }
    }
}