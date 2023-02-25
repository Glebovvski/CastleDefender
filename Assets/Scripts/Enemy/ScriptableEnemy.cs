using Managers;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "SO_Enemy_", menuName = "ScriptableEnemy", order = 0)]
    public class ScriptableEnemy : ScriptableObject
    {
        public int Health;
        public int AttackForce;
        public int GoldToDrop;
        public int Speed;
        public float ScanRange;
        public float AttackRange;
        public float AttackRadius;
        public float TimeBetweenAttacks;

        [Space(10)]
        public int AttackWallScore;
        public int AttackCannonScore;
        public int AttackLaserScore;
        public int AttackCastleScore;
        public int AttackTrapScore;
        public EnemyType EnemyType;

        [Space(10)]
        public PoolObjectType Type;
    }
}