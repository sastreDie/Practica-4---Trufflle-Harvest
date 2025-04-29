using System;

namespace Platformer
{
    public class EntitySpawner<T> where T : Entity
    {
        IEntityFactory<T> entityFactory;
        iSpawnPointStrategy spawnPointStrategy;

        public EntitySpawner(IEntityFactory<T> entityFactory, iSpawnPointStrategy spawnPointStrategy)
        {
            this.entityFactory = entityFactory;
            this.spawnPointStrategy = spawnPointStrategy;
        }

        public T Spawn()
        {
            return entityFactory.Create(spawnPointStrategy.NextSpawnPoint());
        }

    }
}