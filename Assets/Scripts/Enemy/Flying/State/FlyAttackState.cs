using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Data;
using Infastructure;
using Player;
using Services;
using UI.Enemy.Flying;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Enemy.State
{
    public class FlyAttackState : IPayloadedState<EnemyFlying>
    {
        private FlyingStateMachine _flyingStateMachine;
        private StaticDataService _staticDataService;
        private UnitTrackerService _unitTrackerService;
        private GameObject _bulletPrefab;
        
        private CancellationTokenSource _cts;
        private EnemyFlying _unit;

        public FlyAttackState(FlyingStateMachine flyingStateMachine)
        {
            _flyingStateMachine = flyingStateMachine;
        }
        
        public void Exit()
        {
            _cts.Cancel();
        }

        public async void Enter(EnemyFlying unit)
        {
            InjectService.Instance.Inject(this);
            _unit = unit;
            _bulletPrefab = Resources.Load<GameObject>("Prefabs/Hero/Bullet");
            
            _cts = new CancellationTokenSource();
            await AttackPlayer(_cts.Token);
        }

        private async UniTask AttackPlayer(CancellationToken cts)
        {
            var agent = _unit.GetAgent();
            var target = _unit.GetAgentTarget();

            while (!cts.IsCancellationRequested)
            {
                await UniTask.Yield();

                if (_unit == null || target == null)
                    return;
                if(agent == null)
                    return;

                var position = target.transform.position;
                _unit.RotateTowardUnit.Target(position);
                var directionToTarget = (position - _unit.transform.position).normalized;

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    var unitPos = _unit.transform.position;
                    var randomAngle = Random.value > 0.5f ? 90f : -90f;
                    var perpendicularDirection = Quaternion.Euler(0, randomAngle, 0) * directionToTarget * 5f;
                    
                    var newPosition = unitPos + perpendicularDirection;
                    var closerPosition = Vector3.MoveTowards(unitPos, target.transform.position, 1f);
                    
                    newPosition = newPosition + closerPosition - unitPos;

                    agent.SetDestination(newPosition);

                    while (agent.remainingDistance > agent.stoppingDistance)
                    {
                        await UniTask.Yield();
                    }

                    Shoot(_unit.CurrentWeapon.SpawnBulletPos.transform.position);
                    await UniTask.Delay(TimeSpan.FromSeconds(2.0));
                }
            }
        }
        
        private void Shoot(Vector3 spawnBullet)
        {
            var bulletGO = 
                GameObject.Instantiate(_bulletPrefab, spawnBullet, _unit.transform.rotation);
            
            bulletGO.GetComponent<Bullet>().SetDamage(_unit.CurrentWeapon.Damage);
            var bullet = bulletGO.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.SetDirection(_unit.transform.forward);
                bullet.SetDamage(_unit.CurrentWeapon.Damage);
            }
        }
    }
}