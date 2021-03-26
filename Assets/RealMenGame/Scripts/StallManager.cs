using RealMenGame.Scripts.Common;
using UniRx;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class StallManager : MonoBehaviourSingleton<StallManager>
    {
        [SerializeField] private Shaurma shaurma;

        [SerializeField] private Transform[] _targets;

        public Transform GetRandomTarget() => _targets[Random.Range(0, _targets.Length)];

        public ReactiveProperty<int> Health = new ReactiveProperty<int>();

        public void SetDamage(int damage)
        {
            Health.Value -= damage;
        }

        public void OnShoot(Vector3 position)
        {
            Shoot(position);
        }

        private void Shoot(Vector3 position)
        {
            shaurma.FlyTo(position);
            shaurma.ResetShaurma();
        }

        public void SetIngredient(IngredientType ingredientType, Ingredient ingredient)
        {
            shaurma.SetIngredient(ingredientType, ingredient);
        }
    }
}