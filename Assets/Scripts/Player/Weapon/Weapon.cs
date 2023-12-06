using Data;
using Infastructure;
using UnityEngine;

namespace Player
{
    public class Weapon : MonoBehaviour
    {
        public WeaponType Type;
        public int Damage { get; protected set; }
        public float AttackSpeed { get; protected set; }
        public Transform SpawnBulletPos { get; protected set; }

        private GameObject _unit;
        private bool _isFollow = false;

        public void SetFollowPlayer(GameObject player)
        {
            _unit = player;
            _isFollow = true;
        }

        private void Update()
        {
            if (_isFollow)
            {
                transform.position = _unit.transform.position + _unit.transform.forward;
                transform.rotation = Quaternion.LookRotation(_unit.transform.forward);
            }
        }
    }
}