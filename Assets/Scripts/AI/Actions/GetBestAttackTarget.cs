using System.Collections.Generic;
using System.Linq;
using AI;
using Apex.AI;
using Apex.Serialization;
using Enemies;
using UnityEngine.AI;

public class GetBestAttackTarget : ActionBase
{
    [ApexSerialization] private int MaxEnemiesToAttackOneTarget = 3;
    List<TargetScore> scores = new List<TargetScore>();
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        if (!enemy.IsAlive) return;

        Observation attackTarget = null;

        if (enemy.AttackTarget != null) return;
        int pathScore = 0;
        foreach (var defense in enemy.DefenseTypeToScore)
        {
            var bestDefenseType = defense;
            var closestObservation = Enemies.AIManager.Instance.GetClosestObservationByType(enemy, defense.Key);
            if (closestObservation == null)
                continue;
            var offset = (closestObservation.Position - enemy.transform.position).normalized/2f;
            closestObservation.SetPosition(offset);
            var path = enemy.GetCalculatedPath(closestObservation);
            if (path.status != NavMeshPathStatus.PathComplete)
                continue;
            var enemiesWithSameTarget = Enemies.AIManager.Instance.GetEnemiesAttackingObservation(closestObservation).Count();
            scores.Add(new TargetScore(path.corners.Length, enemiesWithSameTarget, enemy, closestObservation, defense.Value + pathScore, MaxEnemiesToAttackOneTarget));
            pathScore = 0;
        }
        if (scores.Count() != 0)
            attackTarget = scores.OrderByDescending(x => x.TotalScore).First().Observation;
        else
            attackTarget = Enemies.AIManager.Instance.GetClosest(c.Enemy);

        if (attackTarget == null) return;
        c.Enemy.SetAttackTarget(attackTarget);
        scores.Clear();
    }
}

public class TargetScore
{
    private int pathLengthScore;
    private int enemiesAttackingTarget;
    private Enemy enemy;
    private int extraScore;
    private int maxEnemies;
    public Observation Observation { get; private set; }

    public int TotalScore => GetTotalScore();

    public TargetScore(int pathLength, int enemiesAttackingTarget, Enemy enemy, Observation observation, int extraScore, int maxEnemies)
    {
        pathLengthScore = 100 - pathLength;
        this.enemiesAttackingTarget = enemiesAttackingTarget;
        this.enemy = enemy;
        Observation = observation;
        this.extraScore = extraScore;
        this.maxEnemies = maxEnemies;
    }

    public int GetTotalScore()
    {
        return pathLengthScore + GetScoreForEnemiesAttackingTarget() + extraScore;
    }

    private int GetScoreForEnemiesAttackingTarget()
    {
        int score = 0;
        var type = enemy.EnemyType;
        switch (type)
        {
            case EnemyType.Mono:
                {
                    score = 0;
                    break;
                }
            case EnemyType.Any:
                {
                    score = IsHealthLowScore();
                    break;
                }
            case EnemyType.Team:
                {
                    var enemiesAttackingTargetScore = enemiesAttackingTarget <= maxEnemies ? enemiesAttackingTarget : -enemiesAttackingTarget * 100;
                    score = enemiesAttackingTargetScore * 100 - IsHealthLowScore();
                    break;
                }
            case EnemyType.Kamikaze:
                {
                    score = IsHealthLowScore() * -100;
                    break;
                }
        }
        return score;
    }

    private int IsHealthLowScore() => Observation.Defense.CurrentHealth / Observation.Defense.Health <= 0.3f ? 100 : 0;

}
