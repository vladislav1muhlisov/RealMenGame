using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace RealMenGame.Scripts.UI
{
    public class NewLevelPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            InstanceOnOnNewLevelSet(0);
            GameManager.Instance.OnNewLevelSet += InstanceOnOnNewLevelSet;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnNewLevelSet -= InstanceOnOnNewLevelSet;
        }

        private void InstanceOnOnNewLevelSet(int level)
        {
            _text.text = $"Волна {level + 1}";
            Show().Forget();
        }

        private async UniTask Show()
        {
            _panel.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(3));
            _panel.SetActive(false);
        }
    }
}