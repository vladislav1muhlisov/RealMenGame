using System.Collections.Generic;
using DG.Tweening;
using RealMenGame.Scripts.Common;
using UniRx;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class StallManager : MonoBehaviourSingleton<StallManager>
    {
        [SerializeField] private Shaurma shaurma;

        [SerializeField] private Transform[] _targets;
        [SerializeField] private KebabProjectile _kebabProjectilePrefab;
        [SerializeField] private Transform _kebabStartPosition;
        [SerializeField] private float _kebabSpeed = 100f;

        private const float DistanceToFly = 1000f;

        public Transform GetRandomTarget() => _targets[Random.Range(0, _targets.Length)];

        public ReactiveProperty<int> Health = new ReactiveProperty<int>(MaxHealth);
        public ReactiveProperty<int> Score = new ReactiveProperty<int>();
        public const int MaxHealth = 100;

        public void SetDamage(int damage)
        {
            Health.Value -= damage;
        }

        public void OnShoot(Vector3 position)
        {
            if (shaurma.Ingredients.Count != 4) return;
            
            var kebabPosition = _kebabStartPosition.position;
            var kebabProjectile = Instantiate(_kebabProjectilePrefab, kebabPosition, Quaternion.identity);

            kebabProjectile.Ingredients.Ingredients = new Dictionary<IngredientType, Ingredient>(shaurma.Ingredients);

            position = new Vector3(position.x, kebabPosition.y, position.z);

            var dir = (position - kebabPosition).normalized;

            kebabProjectile.TweenHandle = kebabProjectile.transform.DOMove(dir * DistanceToFly, DistanceToFly / _kebabSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() => Destroy(kebabProjectile.gameObject));
            
            shaurma.ResetShaurma();
        }

        public void SetIngredient(IngredientType ingredientType, Ingredient ingredient)
        {
            shaurma.SetIngredient(ingredientType, ingredient);
        }
    }
}