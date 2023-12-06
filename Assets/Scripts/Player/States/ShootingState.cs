using System.Threading;
using Cysharp.Threading.Tasks;
using Data;
using DefaultNamespace;
using Infastructure;
using Infastructure.Services;
using Services;
using UnityEngine;
using Zenject;

namespace Player.States
{
    public class ShootingState : IPayloadedState<GameObject>
    {
        private HeroStateMachine _heroStateMachine;
        
        private UnitSpawnService _unitSpawnService;
        private StaticDataService _staticDataService;
        private IInputService _inputService;
        private CancellationTokenSource _cts;

        private Hero _hero;
        private GameObject _targer;
        private GameObject _bulletPrefab;
        private float _lastShotTime;
        
        [Inject]
        public void Construct(UnitSpawnService unitSpawnService,
            StaticDataService staticDataService)
        {
            _unitSpawnService = unitSpawnService;
            _staticDataService = staticDataService;
        }
        
        public ShootingState(HeroStateMachine heroStateMachine)
        {
            _heroStateMachine = heroStateMachine;
        }

        public async void Enter(GameObject unit)
        {
            InjectService.Instance.Inject(this);
            _inputService = Game.InputService;
            _targer = unit;

            _hero = _unitSpawnService._player.GetComponent<Hero>();
            _bulletPrefab = Resources.Load<GameObject>("Prefabs/Hero/Bullet");

            _cts = new CancellationTokenSource();
            await ShootingEnemy(_cts.Token);
        }
        
        public void Exit()
        {
            _cts?.Cancel();
        }
        
        private bool CanShoot()
        {
            return Time.time - _lastShotTime >= _hero.CurrentWeapon.AttackSpeed;
        }
        
        private void Shoot(Vector3 spawnBullet)
        {
            var bulletGO = 
                GameObject.Instantiate(_bulletPrefab, _hero.CurrentWeapon.SpawnBulletPos.position, Quaternion.identity);
            
            bulletGO.GetComponent<Bullet>().SetDamage(_hero.CurrentWeapon.Damage);
            var bullet = bulletGO.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.SetDirection(_unitSpawnService._player.transform.forward);
                bullet.SetDamage(_hero.CurrentWeapon.Damage);
            }
        }

        private async UniTask ShootingEnemy(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                
                if(_targer != null)
                    _hero.RotateTowardUnit.Target(_targer.transform.position);
                
                if (CanShoot())
                {
                    Shoot(_unitSpawnService._player.transform.position);
                    _lastShotTime = Time.time;
                }
                
                if(_inputService.Axis.sqrMagnitude > Constants.EPSILON)
                {
                    _heroStateMachine.Enter<MovingState>();
                    return;
                }
                if (_targer == null)
                {
                    _heroStateMachine.Enter<FindEnemyState>();
                    return;
                }
            }
        }
    }
}