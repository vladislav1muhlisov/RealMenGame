using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RealMenGame.Scripts.Common;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RealMenGame.Scripts
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        [NonSerialized] public ReactiveProperty<int> Score = new ReactiveProperty<int>(0);
        [NonSerialized] public ReactiveProperty<int> Health = new ReactiveProperty<int>(MaxHealth);

        [SerializeField] private CanvasGroup _fullScreenFade;
        
        public const int MaxHealth = 100;

        public void SetDamage(int damage)
        {
            Health.Value -= damage;
            if (Health.Value <= 0)
            {
                SceneManager.LoadScene("ShaurmaGameOver");
            }
        }

        public async void NewGame()
        {
            await _fullScreenFade.DOFade(1f, 0.4f);
            
            Score.Value = 0;
            Health.Value = MaxHealth;
            
            await SceneManager.LoadSceneAsync("Shaurmyachnaya");
            await _fullScreenFade.DOFade(0f, 0.4f);
        }
    }
}