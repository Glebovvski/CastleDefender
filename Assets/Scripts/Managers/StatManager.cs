using Zenject;

namespace Managers
{
    public class StatManager : IInitializable
    {
        private int enemiesKilled = 0;
        private int defenseDestroyed = 0;

        public int EnemiesKilled { get => enemiesKilled; private set => enemiesKilled = value; }
        public int DefensesDestroyed { get => defenseDestroyed; private set => defenseDestroyed = value; }

        public void AddEnemiesKilled() => EnemiesKilled++;
        public void AddDefensesDestroyed() => DefensesDestroyed++;

        public void Initialize()
        {
            Reset();
        }

        public void Reset()
        {
            enemiesKilled = 0;
            defenseDestroyed = 0;
        }
    }
}