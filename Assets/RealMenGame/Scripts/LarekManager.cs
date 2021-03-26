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

        public void OnShoot(Transform target)
        {
            Shoot(target).Forget();
        }

        private async UniTask Shoot(Transform target)
        {
            IsShooting = true;
            await shaurma.FlyTo(target);
            shaurma.ResetShaurma();
            IsShooting = false;
        }

        public void AddIngredient(IngredientType ingredientType)
        {
            shaurma.AddIngredient(ingredientType);
        }
    }
}