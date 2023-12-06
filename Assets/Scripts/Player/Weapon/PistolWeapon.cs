using System;
using Infastructure;
using Services;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PistolWeapon : Weapon
    {
        [SerializeField] private Transform _spawnBulletPosition;

        public PistolWeapon(int damage, float attackSpeed)
        {
            Initialize(damage, attackSpeed);
        }

        private void Start()
        {
            SpawnBulletPos = _spawnBulletPosition;
        }

        public void Initialize(int damage, float attackSpeed)
        {
            Damage = damage;
            AttackSpeed = attackSpeed;
        }
    }
}