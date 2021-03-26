using Cysharp.Threading.Tasks;
using RealMenGame.Scripts.Common;
using UniRx;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class LarekManager : MonoBehaviourSingleton<LarekManager>
    {
        [SerializeField] private Shaurma shaurma;
        
        public bool IsShooting { get; private set; }

        public ReactiveProperty<int> Health = new ReactiveProperty<int>();


        public void SetDamage(int damage)
        {
            Health.Value -= damage;
        }

        public void OnShoot(Vector3 position)
        {
            Shoot(position).Forget();
        }

        private async UniTask Shoot(Vector3 position)
        {
            IsShooting = true;
            await shaurma.FlyTo(position);
            shaurma.ResetShaurma();
            IsShooting = false;
        }

        public void SetIngredient(IngredientType ingredientType, int number)
        {
            shaurma.SetIngredient(ingredientType, number);
        }
    }
}