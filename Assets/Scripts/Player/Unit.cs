using Data;
using UnityEngine;

namespace Player
{
    public abstract class Unit : MonoBehaviour
    {
        public virtual UnitType Type { get; set; }
        public virtual float Health { get; set; }
        public virtual float Speed  { get; set; }
        public virtual int EnemyPrice  { get; set; }
        
        public abstract void TakeDamage(float damage);
    }
}