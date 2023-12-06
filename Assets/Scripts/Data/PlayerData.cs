using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public GameObject PlayerPrefab;
        public Vector3 PlayerPosition;
        public float Health;
        public float Speed;
    }
}