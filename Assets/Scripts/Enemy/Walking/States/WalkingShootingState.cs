using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Infastructure;
using Player;
using Player.States;
using Services;
using UI.Enemy;
using UnityEngine;
using Zenject;

namespace Enemy.Walking
{
    public class WalkingShootingState : IPayloadedState<EnemyWalking>
    {
        private EnemyWalkingStateMachine _enemyWalkingStateMachine;
        private StaticDataService _staticDataService;
        
        private CancellationTokenSource _cts;
        private GameObject _bulletPrefab;
        private EnemyWalking _unit;
        private float _lastShotTime;

        [Inject]
        public void Construct(StaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public WalkingShootingState(EnemyWalkingStateMachine enemyWalkingStateMachine)
        {
            _enemyWalkingStateMachine = enemyWalkingStateMachine;
        }

        public void Exit()
        {
            _cts.Cancel();
        }

        public async void Enter(EnemyWalking unit)
        {
            InjectService.Instance.Inject(this);
            _bulletPrefab = Resources.Load<GameObject>("Prefabs/Hero/Bullet");
            _unit = unit;
            
            _cts = new CancellationTokenSource();
            await Shooting(_cts.Token);
        }

        private async UniTask Shooting(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                
                if (CanShoot() && _unit != null)
                {
                    Shoot(_unit.CurrentWeapon.SpawnBulletPos.position);
                    _lastShotTime = Time.time;
                }
            }
        }
        
        private void Shoot(Vector3 spawnBullet)
        {
            var bulletGO = 
                GameObject.Instantiate(_bulletPrefab, spawnBullet, Quaternion.identity);
            
            bulletGO.GetComponent<Bullet>().SetDamage(_unit.CurrentWeapon.Damage);
            var bullet = bulletGO.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.SetDirection(_unit.transform.forward);
                bullet.SetDamage(_unit.CurrentWeapon.Damage);
            }
        }
        
        private bool CanShoot()
        {
            return Time.time - _lastShotTime >= _unit.CurrentWeapon.AttackSpeed;
        }
    }
}