using System;
using Data;
using Infastructure;
using Services;
using UnityEngine;
using Zenject;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        private float Damage;
        private float bulletSpeed = 20.0f; 
        private Vector3 bulletDirection;

        [Inject] private StaticDataService _staticDataService;

        public void Start()
        {
            InjectService.Instance.Inject(this);
        }

        public void SetDirection(Vector3 _direction)
        {
            //bulletDirection = _direction.normalized;
            //transform.rotation = Quaternion.LookRotation(bulletDirection);
            bulletDirection = _direction.normalized;
            transform.localRotation = Quaternion.LookRotation(bulletDirection);
        }
        
        public void SetDamage(float damage)
        {
            Damage = damage;
        }

        private void Update()
        {
            MoveBullet();
        }

        private void MoveBullet()
        {
            transform.position += bulletDirection * bulletSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider col)
        {
            var particle = _staticDataService.Paricles[ParticleType.Explosion].Particle;
            Instantiate(particle, gameObject.transform.position, Quaternion.identity);
            
            var unit = col.GetComponent<Unit>();
            if (unit != null)
            {
                unit.TakeDamage(Damage);
            }
            
            Destroy(gameObject);
        }
    }
}