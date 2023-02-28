using System.Collections.Generic;
using Apex.AI;
using Enemies;
using UnityEngine;
using Defendable;

namespace AI
{
    public class ScanAction : ActionBase
    {
        private LayerMask LayerMask;
        int Corner = 0;
        List<Observation> observations = new List<Observation>();

        public override void Execute(IAIContext context)
        {
            LayerMask = LayerMask.GetMask("Defense");
            var c = (AIContext)context;
            var Enemy = (SpyEnemy)c.Enemy;

            var colliders = Physics.OverlapSphere(Enemy.Position, Enemy.ScanRange, LayerMask);
            foreach (var defence in colliders)
            {
                observations.Add(new Observation(defence.GetComponent<Defense>()));
            }
            Enemies.AIManager.Instance.AddObservation(observations);
            observations.Clear();

            Enemy.SetIsScanFinished(true);
        }
    }
}