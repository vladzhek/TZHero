using System.Collections.Generic;
using Data;
using Player;
using Services;
using UnityEngine;

namespace DefaultNamespace
{
    public class UnitSpawnService
    {
        private StaticDataService _staticDataService;
        public Dictionary<GameObject, UnitType> _enemies { get; set; } = new();
        public GameObject _player { get; set; }

        public UnitSpawnService(StaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Spawn()
        {
            SpawnPlayer();
            SpawnEnemy();
        }
        
        private void SpawnPlayer()
        {
            var data = _staticDataService.PlayerData;
            _player = Object.Instantiate(data.PlayerPrefab, data.PlayerPosition, Quaternion.identity);
        }
        
        private void SpawnEnemy()
        {
            var enemyConfig = _staticDataService.LevelData.LevelConfigs[0].EnemyConfig;
            foreach (var enemy in enemyConfig)
            {
                if(enemy.unitType == UnitType.Player)
                    continue;

                var prefab = Object.Instantiate(enemy.EnemyData.EnemyPrefab, enemy.SpawnPos, Quaternion.identity);
                _enemies.Add(prefab, enemy.unitType);
            }
        }
    }
}