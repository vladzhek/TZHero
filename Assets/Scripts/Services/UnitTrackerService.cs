using DefaultNamespace;
using Infastructure;
using Player;
using UnityEngine;

namespace Services
{
    public class UnitTrackerService
    {
        private UnitSpawnService _unitSpawnService;
        public GameObject Target;

        public UnitTrackerService(UnitSpawnService unitSpawnService)
        {
            _unitSpawnService = unitSpawnService;
        }

        public GameObject GetEnemyTarget()
        {
            var player = _unitSpawnService._player;
            foreach (var (enemy ,type) in _unitSpawnService._enemies)
            {
                if(enemy == null)
                    break;
                    
                var distance = Vector3.Distance(player.transform.position, enemy.transform.position);
                if (distance <= Constants.PLAYER_TARGET_RANGE) 
                {
                    return enemy;
                }
            }

            return null;
        }
        
        public Hero GetPlayerTarget(Vector3 enemyPos)
        {
            var player = _unitSpawnService._player.GetComponent<Hero>();
            var distance = Vector3.Distance(player.transform.position, enemyPos);
            if (distance <= Constants.ENEMY_TARGET_RANGE) 
            {
                return player;
            }
            
            return null;
        }
    }
}