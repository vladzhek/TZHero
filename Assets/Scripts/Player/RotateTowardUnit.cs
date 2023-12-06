using UnityEngine;

namespace Player
{
    public class RotateTowardUnit
    {
        private GameObject _unit;
        public RotateTowardUnit(GameObject unit)
        {
            _unit = unit;
        }

        public void Target(Vector3 target)
        {
            Vector3 direction = target - _unit.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            _unit.transform.rotation = rotation;
        }
        
        public void LerpRotate(Vector3 target)
        {
            Vector3 direction = target - _unit.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Используем Quaternion.RotateTowards для постепенного поворота
            _unit.transform.rotation = 
                Quaternion.RotateTowards(_unit.transform.rotation, rotation, 2 * Time.deltaTime);
        }
    }
}