using System;
using Data;
using Infastructure;
using Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coins;
        private CurrencyService _currencyService;

        [Inject]
        public void Construct(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        void Start()
        {
            InjectService.Instance.Inject(this);
            _coins.text = _currencyService.SoftCoins.ToString();
            _currencyService.OnCollect += UpdateCoin;
        }

        void UpdateCoin(CurrencyType type, int amount)
        {
            _coins.text = _currencyService.SoftCoins.ToString();
        }
    }
}