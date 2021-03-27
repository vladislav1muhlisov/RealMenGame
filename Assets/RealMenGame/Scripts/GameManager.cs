﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RealMenGame.Scripts.Common;
using RealMenGame.Scripts.Settings;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RealMenGame.Scripts
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        [NonSerialized] public readonly ReactiveProperty<int> Score = new ReactiveProperty<int>(0);
        [NonSerialized] public readonly ReactiveProperty<int> Health = new ReactiveProperty<int>();
        public event Action<int> OnNewLevelSet = delegate { };

        [SerializeField] private CanvasGroup _fullScreenFade;
        [SerializeField] private LevelsSettings _levelsSettings;
        public LevelSettings _currentLevel;
        private int _currentLevelIndex;

        public int MaxHealth => _currentLevel.MaxHealth;

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            if (SceneManager.GetActiveScene().name == "Shaurmyachnaya")
            {
                NewGame();
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public void SetDamage(int damage)
        {
            Health.Value -= damage;
        }

        public void NewGame()
        {
            Score.Value = 0;
            CrossFadeToScene("Shaurmyachnaya", () => SetLevel(0), () => OnNewLevelSet?.Invoke(0)).Forget();
        }

        private void NextLevel()
        {
            CrossFadeToScene("Shaurmyachnaya", () => SetLevel((_currentLevelIndex + 1) % _levelsSettings.Levels.Length),
                    () => OnNewLevelSet?.Invoke(_currentLevelIndex))
                .Forget();
        }

        private void GameOver()
        {
            CrossFadeToScene("ShaurmaGameOver").Forget();
        }

        private void SetLevel(int index)
        {
            _currentLevelIndex = index;
            _currentLevel = _levelsSettings.Levels[_currentLevelIndex];

            Health.Value = _currentLevel.MaxHealth;

            Score.Where(v => v >= _currentLevel.NextLevelScore).Take(1).Subscribe(_ => NextLevel());
            Health.Where(v => v <= 0).Take(1).Subscribe(_ => GameOver());
        }

        private async UniTask CrossFadeToScene(string sceneName, Action action = null, Action postAction = null)
        {
            await _fullScreenFade.DOFade(1f, 0.4f);

            action?.Invoke();

            await SceneManager.LoadSceneAsync(sceneName);
            await _fullScreenFade.DOFade(0f, 0.4f);

            postAction?.Invoke();
        }
    }
}