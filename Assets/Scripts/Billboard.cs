using System;
using Infastructure;
using Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Unit _unit;

        private void Start()
        {
            _healthSlider.maxValue = _unit.Health;
        }

        private void LateUpdate()
        {
            _healthSlider.value = _unit.Health;
            Transform cam = Camera.main.transform;
            transform.LookAt(transform.position + cam.forward);
        }
    }
}