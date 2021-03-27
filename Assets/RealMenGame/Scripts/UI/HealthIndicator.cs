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
            GameManager.Instance.Health.Subscribe(OnHealthChanged).AddTo(_compositeDisposable);
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }

        private void OnHealthChanged(int health)
        {
            var heartsToActivate = Mathf.CeilToInt((float) health * _hearts.Length / GameManager.MaxHealth);
            for (int i = 0; i < _hearts.Length; i++)
            {
                _hearts[i].SetActive(heartsToActivate > i);
            }
        }
    }
}