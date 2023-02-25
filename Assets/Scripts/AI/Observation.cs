using System;
using Defendable;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public class Observation : IEquatable<Observation>
    {
        public Observation(Defense defence)
        {
            this.Defense = defence;
        }

        public Defense Defense { get; private set; }
        public Vector3 Position => Defense.gameObject.transform.position - offset;
        public Vector3 Size => Defense.transform.localScale;
        public bool IsAlive => Defense.CurrentHealth > 0;

        private Vector3 offset;

        public bool Equals(Observation other)
        {
            return this.Defense == other.Defense;
        }

        internal void SetPosition(Vector3 vector3)
        {
            offset = vector3;
        }

        internal void SetAsAttackTarget()
        {
            Defense.SetAsTargetToAttack();
        }
    }
}