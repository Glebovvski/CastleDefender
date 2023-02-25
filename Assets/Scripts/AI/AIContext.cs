using Apex.AI;
using Enemies;

namespace AI
{
    public class AIContext : IAIContext
    {
        public Enemy Enemy { get; private set; }
        public AIContext(Enemy enemy)
        {
            Enemy = enemy;
        }
    }
}