using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public GameObject EnemyPrefab;
        public float Health;
        public float Speed;
        public float Damage;
        public float AttackSpeed;
        public int EnemyPrice;
    }
}