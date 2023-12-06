using System;
using Infastructure;
using Infastructure.Services;
using UnityEngine;

namespace Player
{
    public class HeroMove : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        private float _movementSpeed;
        
        private IInputService _inputService;
        private Camera _camera;

        public void Awake()
        {
            _inputService = Game.InputService;
        }

        public void Start()
        {
            _camera = Camera.main;
        }

        public void FixedUpdate()
        {
            Vector3 moveVector = Vector3.zero;
            var unit = gameObject.GetComponent<Unit>();
            if(unit != null)
                _movementSpeed = unit.Speed;

            if (_inputService.Axis.sqrMagnitude > Constants.EPSILON)
            {
                moveVector = _camera.transform.TransformDirection(_inputService.Axis);
                moveVector.y = 0;
                moveVector.Normalize();

                transform.forward = moveVector;
            }

            moveVector += Physics.gravity;
            _characterController.Move( _movementSpeed * moveVector * Time.deltaTime);
        }
    }
}