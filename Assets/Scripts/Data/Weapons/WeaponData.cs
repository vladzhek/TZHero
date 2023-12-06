using Player;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public WeaponType Type;
        public GameObject WeaponPrefab;
        public int Damage;
        public float AttackSpeed;
    }
}