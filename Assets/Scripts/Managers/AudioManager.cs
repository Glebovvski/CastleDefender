using Defendable;
using Enemies;
using Grid;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        private GameGrid Grid { get; set; }

        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip ui;
        [SerializeField] private AudioClip explosion;
        [SerializeField] private AudioClip fire;
        [SerializeField] private AudioClip destroy_enemy;
        [SerializeField] private AudioClip destroy_defense;

        [Inject]
        private void Construct(GameGrid grid)
        {
            Grid = grid;
        }

        private void Start()
        {
            AIManager.Instance.OnEnemySpawn += SetEnemySounds;
            AIManager.Instance.OnEnemyKilled += ResetEnemySounds;

            Grid.OnDefenseSet += SetDefenseSound;
            Grid.OnResetDefense += ResetDefenseSound;
        }

        public void PlayUI() => Play(ui);

        private void SetDefenseSound(Defense defense)
        {
            Play(ui);
            defense.OnDeath += () => Play(destroy_defense);
        }

        private void ResetDefenseSound(Defense defense)
        {
            defense.OnDeath -= () => Play(destroy_defense);
        }

        private void SetEnemySounds(Enemy enemy, PoolObjectType type)
        {
            enemy.OnDeath += (enemy) => Play(destroy_enemy);
            if (type == PoolObjectType.KamikazeEnemy)
                ((KamikazeEnemy)enemy).OnExplode += () => Play(explosion);
        }

        private void ResetEnemySounds(Enemy enemy, PoolObjectType type)
        {
            enemy.OnDeath -= (enemy) => Play(destroy_enemy);
            if (type == PoolObjectType.KamikazeEnemy)
                ((KamikazeEnemy)enemy).OnExplode -= () => Play(explosion);
        }

        private void Play(AudioClip clip) => source.PlayOneShot(clip);

        private void OnDestroy()
        {
            AIManager.Instance.OnEnemySpawn -= SetEnemySounds;
            AIManager.Instance.OnEnemyKilled -= ResetEnemySounds;
        }
    }
}