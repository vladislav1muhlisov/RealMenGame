using RealMenGame.Scripts.Common;
using UniRx;
using UnityEngine.SceneManagement;

namespace RealMenGame.Scripts
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public ReactiveProperty<int> Score = new ReactiveProperty<int>();
        public ReactiveProperty<int> Health = new ReactiveProperty<int>(MaxHealth);
        public const int MaxHealth = 100;

        public void SetDamage(int damage)
        {
            Health.Value -= damage;
            if (Health.Value <= 0)
            {
                SceneManager.LoadScene("ShaurmaGameOver");
            }
        }

        public void NewGame()
        {
            Score.Value = 0;
            Health.Value = MaxHealth;
            SceneManager.LoadScene("Shaurmyachnaya");
        }
    }
}