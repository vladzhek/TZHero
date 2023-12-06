using Data;
using DefaultNamespace;
using Infastructure;
using Player;
using Services;
using UI.Enemy.State;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Unit = Player.Unit;

namespace UI.Enemy.Flying
{
    public class EnemyFlying : Unit
    {
        [SerializeField] private Transform WeaponSpawnPosition;
        [SerializeField] private NavMeshAgent _agent;
        public GameObject WeaponPrefab { get; set; }
        public RotateTowardUnit RotateTowardUnit;
        public Weapon CurrentWeapon { get; set; }
        public override UnitType Type  => _type;
        public override float Health  => _health;
        public override float Speed  => _speed;
        public override int EnemyPrice  => _enemyPrice;
        

        private UnitType _type = UnitType.EnemyFlying;
        private float _health { get; set; }
        private float _speed { get; set; }
        private int _enemyPrice { get; set; }

        private readonly FlyingStateMachine _flyingStateMachine;
        private PlayerData PlayerDataProgress;

        [Inject] private StaticDataService _staticDataService;
        [Inject] private CurrencyService _currencyService;
        [Inject] private UnitSpawnService _unitSpawnService;
        
        private EnemyStruct _enemyRider;
        private GameObject _target;
        

        public EnemyFlying()
        {
            _flyingStateMachine = new FlyingStateMachine();
        }

        void Awake()
        {
            InjectService.Instance.Inject(this);
            Initialize();

            _agent.stoppingDistance = 1f;
            
            RotateTowardUnit = new RotateTowardUnit(gameObject);
            _flyingStateMachine.Enter<FlyFindPlayerState, EnemyFlying>(this);
        }

        private void Update()
        {
            if(_target != null)
                RotateTowardUnit.Target(_target.transform.position);
        }

        private void Initialize()
        {
            _enemyRider = _staticDataService
                .LevelData.LevelConfigs[0]
                .EnemyConfig.Find(x => x.unitType == Type);

            _health = _enemyRider.EnemyData.Health;
            _speed = _enemyRider.EnemyData.Speed;
            _enemyPrice = _enemyRider.EnemyData.EnemyPrice;
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
        
        public void SetAgentTarget(GameObject target)
        {
            _target = target;
        }

        public GameObject GetAgentTarget()
        {
            return _target;
        }
        
        public NavMeshAgent GetAgent()
        {
            return _agent;
        }

        public override void TakeDamage(float damage)
        {
            _health -= damage;
            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            _currencyService.AddCurrency(CurrencyType.Soft, EnemyPrice);
            _unitSpawnService._enemies.Remove(gameObject);
            Destroy(WeaponPrefab);
            Destroy(gameObject);
        }
    }
}