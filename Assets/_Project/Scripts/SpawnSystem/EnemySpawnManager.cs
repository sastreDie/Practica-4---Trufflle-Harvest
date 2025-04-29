using System;
using UnityEngine;
using Utilities;

namespace Platformer
{
    public class EnemySpawnManager : EntitySpawnManager
    {
        [SerializeField] EnemyData[] enemyData;
        [SerializeField] float spawnInterval = 1f;
        [SerializeField] int spawnLimit = 25;
        
        EntitySpawner<Enemy> spawner;
        
        CountdownTimer spawnTimer;
        private int counter;

        protected override void Awake()
        {
            base.Awake();

            spawner = new EntitySpawner<Enemy>(new EntityFactory<Enemy>(enemyData), spawnPointStrategy);
            
            spawnTimer = new CountdownTimer(spawnInterval);
            spawnTimer.OnTimerStop += () =>
            {
                
                if (counter < spawnLimit)
                {
                    Spawn();
                }
                spawnTimer.Start();
            };
        }
        
        private void OnEnable()
        {
            Enemy.OnEnemyDied += HandleEnemyDeath;
        }

        private void OnDisable()
        {
            Enemy.OnEnemyDied -= HandleEnemyDeath;
        }

        private void HandleEnemyDeath()
        {
            counter--;
        }
        
        public void ResetSpawner()
        {
            counter = 0;
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                Destroy(enemy);
            }
        }


        private void Start() => spawnTimer.Start();

        void Update() => spawnTimer.Tick(Time.deltaTime);

        public override void Spawn()
        {
            spawner.Spawn();
            counter++;
        }
    }
}