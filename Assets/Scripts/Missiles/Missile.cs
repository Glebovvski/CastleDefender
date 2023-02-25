using UnityEngine;

namespace Missiles
{
    public abstract class Missile : MonoBehaviour, IMissile
    {
        [SerializeField] private ScriptableMissile SO;
        public Vector3 Direction { get; private set; } = Vector3.zero;
        public int Damage => SO.Damage;
        public float Speed => SO.Speed;
        public bool IsFromPool => SO.IsFromPool;
        public float HitRadius => SO.HitRadius;

        public virtual void Fire(Vector3 direction)
        {
            Direction = direction;
        }

        private void Update()
        {
            transform.position += Direction * Time.deltaTime * Speed;
        }

        public abstract void OnTriggerEnter(Collider other);
    }
}