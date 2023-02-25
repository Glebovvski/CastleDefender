namespace Enemies
{
    public class SpyEnemy : Enemy
    {
        public bool IsScanFinished { get; private set; } = false;

        public override void StartAttack(out bool isReady)
        {
            base.StartAttack(out isReady);
        }

        public void SetIsScanFinished(bool value) => IsScanFinished = value;
    }
}