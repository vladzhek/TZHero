using Data;
using DefaultNamespace;
using Enemy;
using Enemy.Walking;
using Infastructure;
using Player;
using Services;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Unit = Player.Unit;

namespace UI.Enemy
{
    public class EnemyWalking : Unit
    {
        [SerializeField] private Transform WeaponSpawnPosition;
        [SerializeField] private NavMeshAgent _agent;
        public GameObject WeaponPrefab { get; set; }
        public Weapon CurrentWeapon { get; set; }
        public override UnitType Type  => _type;
        public override float Health  => _health;
        public override float Speed  => _speed;
        public override int EnemyPrice  => _enemyPrice;

        private UnitType _type = UnitType.EnemyRider;
        private float _health { get; set; }
        private float _speed { get; set; }
        private int _enemyPrice { get; set; }

        private readonly EnemyWalkingStateMachine _enemyWalkingStateMachine;
        private PlayerData PlayerDataProgress;

        [Inject] private StaticDataService _staticDataService;
        [Inject] private CurrencyService _currencyService;
        [Inject] private UnitSpawnService _unitSpawnService;
        private EnemyStruct _enemyRider;
        private GameObject _target;

        public EnemyWalking()
        {
            _enemyWalkingStateMachine = new EnemyWalkingStateMachine();
        }

        void Awake()
        {
            InjectService.Instance.Inject(this);
            Initialize();
            _enemyWalkingStateMachine.Enter<MoveSeekPointState, EnemyWalking>(this);
        }

        private void Initialize()
        {
            _enemyRider = _staticDataService
                .LevelData.LevelConfigs[0]
                .EnemyConfig.Find(x => x.unitType == Type);

            _health = _enemyRider.EnemyData.Health;
            _speed = _enemyRider.EnemyData.Speed;
            _enemyPrice = _enemyRider.EnemyData.EnemyPrice;
            var weapon = _staticDataService.Weapons[WeaponType.EnemyPistol];
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

        public void Update()
        {
            if (_target != null)
            {
                _agent.SetDestination(_target.transform.position);
            }
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