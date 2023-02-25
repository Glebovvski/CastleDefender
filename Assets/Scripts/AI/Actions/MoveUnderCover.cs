using System.Linq;
using AI;
using Apex.AI;
using Enemies;
using UnityEngine;

public class MoveUnderCover : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var closeEnemies = Enemies.AIManager.Instance.GetClosestEnemiesWithSameTarget(enemy).ToList();
        var tankEnemy = Enemies.AIManager.Instance.GetClosestEnemyByType(enemy, EnemyType.Mono);
        if (closeEnemies.Count > 0 && tankEnemy.isActiveAndEnabled && IsGoingSameWay(tankEnemy, enemy) && enemy.IsNewDestination(tankEnemy.Position))
        {
            enemy.FollowTarget = tankEnemy;
            enemy.MoveTo(enemy.FollowTarget.Position);
        }
    }

    private bool IsGoingSameWay(Enemy tank, Enemy enemy) => Vector3.Dot(tank.transform.forward, enemy.transform.forward) >= 0f;
}
