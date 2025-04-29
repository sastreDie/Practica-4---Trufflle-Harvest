using UnityEngine;

namespace Platformer
{
    public abstract class EntitySpawnManager : MonoBehaviour
    {
        [SerializeField] protected SpawnPointStrategyType spawnPointStrategyType = SpawnPointStrategyType.Random;
        [SerializeField] protected Transform[] spawnPoints;
        
        protected iSpawnPointStrategy spawnPointStrategy;

        protected enum SpawnPointStrategyType
        {
            Random
        }

        protected virtual void Awake()
        {
            spawnPointStrategy = spawnPointStrategyType switch
            {
                SpawnPointStrategyType.Random => new RandomSpawnPointStrategy(spawnPoints),
                _ => spawnPointStrategy
            };
        }
        
        public abstract void Spawn();
    }
}