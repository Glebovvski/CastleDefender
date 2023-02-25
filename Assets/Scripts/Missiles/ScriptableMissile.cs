using UnityEngine;

namespace Missiles
{
    [CreateAssetMenu(fileName = "SO_Missile_", menuName = "ScriptableMissile", order = 0)]
    public class ScriptableMissile : ScriptableObject
    {
        public int Damage;
        public float Speed;
        public bool IsFromPool;
        public float HitRadius;
    }
}