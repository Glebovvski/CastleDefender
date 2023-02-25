using System;

namespace Defendable
{
    public class CastleDefense : Defense
    {
        protected override bool IsReady => true;

        public event Action OnLose;
        public override event Action OnDeath;

        public override void Init(ScriptableDefense so)
        {
            this.gameObject.SetActive(true);
            base.Init(so);
            EnableMesh();
        }

        private void EnableMesh()
        {
            destroyFX.gameObject.SetActive(false);
            defenseMesh.SetActive(true);
        }

        protected override void ReturnToPool()
        {
            DamageReceiver.OnDeath -= OnDeath;
            OnDeath?.Invoke();
        }

        protected override void Death()
        {
            base.Death();
            OnLose?.Invoke();
        }
    }
}