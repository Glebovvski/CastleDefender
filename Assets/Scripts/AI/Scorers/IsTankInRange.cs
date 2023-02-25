using AI;
using Apex.AI;
using Apex.Serialization;
using Enemies;
using UnityEngine;

public class IsTankInRange : ContextualScorerBase
{
    [ApexSerialization] private bool not;
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var tank = Enemies.AIManager.Instance.GetClosestEnemyByType(enemy, Enemies.EnemyType.Mono);
        bool isTankCloser = true;
        if (tank != null && enemy.AttackTarget != null)
            isTankCloser = (tank.Position - enemy.Position).sqrMagnitude < (enemy.AttackTarget.Position - enemy.Position).sqrMagnitude;
        if (tank != null && tank.IsAlive && IsGoingSameWay(tank, enemy) && isTankCloser)
            return not ? -100 : 100;
        return not ? 100 : -100;
    }

    private bool IsGoingSameWay(Enemy tank, Enemy enemy) => Vector3.Dot(tank.transform.forward, enemy.transform.forward) > 0f;
}
