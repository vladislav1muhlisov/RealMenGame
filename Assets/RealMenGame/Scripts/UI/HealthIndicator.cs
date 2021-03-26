using System;
using UniRx;
using UnityEngine;

namespace RealMenGame.Scripts.UI
{
    public class HealthIndicator : MonoBehaviour
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        [SerializeField] private GameObject[] _hearts;

        private void Awake()
        {
            StallManager.Instance.Score.Subscribe(OnHealthChanged).AddTo(_compositeDisposable);
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }

        private void OnHealthChanged(int health)
        {
            var heartsToActivate = Mathf.RoundToInt((float) health * _hearts.Length / StallManager.MaxHealth);
            for (int i = 0; i < _hearts.Length; i++)
            {
                _hearts[i].SetActive(heartsToActivate > i);
            }
        }
    }
}