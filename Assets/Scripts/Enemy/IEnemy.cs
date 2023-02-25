using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public interface IEnemy
    {
        GameObject GameObject { get; }
        NavMeshAgent NavMeshAgent { get; }
        float ScanRange { get; }
        Vector3 Position { get; }

        void MoveTo(Vector3 destination);
    }
}