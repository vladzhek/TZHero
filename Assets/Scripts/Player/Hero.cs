using System;
using Data;
using Infastructure;
using Player.States;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Player
{
    public class Hero : Unit
    {
        [SerializeField] private Transform WeaponSpawnPosition;
        public GameObject WeaponPrefab { get; set; }
        public Weapon CurrentWeapon { get; set; }
        public RotateTowardUnit RotateTowardUnit;
        public override UnitType Type  => _type;
        public override float Health  => _health;
        public override float Speed  => _speed;
        
        
        private UnitType _type = UnitType.Player;
        private float _health { get; set; }
        private float _speed { get; set; }
        private readonly HeroStateMachine _heroStateMachine;
        private PlayerData PlayerDataProgress;

        [Inject] private StaticDataService _staticDataService;

        public Hero()
        {
            _heroStateMachine = new HeroStateMachine();
        }

        void Awake()
        {
            InjectService.Instance.Inject(this);
            RotateTowardUnit = new RotateTowardUnit(gameObject);
            InitialProgress();
            
            _heroStateMachine.Enter<MovingState>();
        }
        
        private void InitialProgress()
        {
            if (PlayerDataProgress == null)
            {
                PlayerDataProgress = _staticDataService.PlayerData;
            }

            _health = PlayerDataProgress.Health;
            _speed = PlayerDataProgress.Speed;
            var weapon = _staticDataService.Weapons[WeaponType.Pistol];
            TakUpArms(weapon);
        }
        
        public void TakUpArms(WeaponData weapon)
        {
            var position = WeaponSpawnPosition.position;
            
            WeaponPrefab = Instantiate(weapon.WeaponPrefab, position, Quaternion.identity);
            var pistol = WeaponPrefab.GetComponent<PistolWeapon>();
            pistol.Initialize(weapon.Damage, weapon.AttackSpeed);
            pistol.SetFollowPlayer(gameObject);
            CurrentWeapon = pistol;
        }
        
        public override void TakeDamage(float damage)
        {
            _health -= damage;
            if (Health <= 0)
                Die();
        }
        
        private void Die()
        {
            
        }
    }
}