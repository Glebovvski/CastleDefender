using System.Collections.Generic;
using Apex.AI;
using Enemies;
using UnityEngine;

namespace AI
{
    public class MoveAroundGrid : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            var enemy = (SpyEnemy)c.Enemy;

            var corners = Enemies.AIManager.Instance.GridCorners();
            int closest = GetClosestCornerIndex(corners, enemy);
            if (enemy.IsNewDestination(corners[closest]))
                enemy.MoveTo(corners[closest]);
            enemy.SetIsScanFinished(false);
        }

        private int GetClosestCornerIndex(List<Vector3> Corners, IEnemy enemy)
        {
            float distance = 1000;
            int cornerIndex = 0;
            for (int i = 0; i < Corners.Count; i++)
            {
                var newDistance = Vector3.Distance(Corners[i], enemy.Position);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    cornerIndex = i;
                }
            }
            if (cornerIndex < 3)
                return cornerIndex + 1;
            else
                return 0;
        }
    }
}