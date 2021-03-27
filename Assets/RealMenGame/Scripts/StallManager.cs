using System.Collections.Generic;
using DG.Tweening;
using RealMenGame.Scripts.Common;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class StallManager : MonoBehaviourSceneSingleton<StallManager>
    {
        [SerializeField] private Shaurma shaurma;

        [SerializeField] private Transform[] _targets;
        [SerializeField] private KebabProjectile _kebabProjectilePrefab;
        [SerializeField] private Transform _kebabStartPosition;
        [SerializeField] private float _kebabSpeed = 100f;

        private const float DistanceToFly = 100f;

        public Transform GetRandomTarget() => _targets[Random.Range(0, _targets.Length)];

        public void OnShoot(Vector3 position)
        {
            var kebabPosition = _kebabStartPosition.position;
            var kebabProjectile = Instantiate(_kebabProjectilePrefab, kebabPosition, Quaternion.identity);

            kebabProjectile.Ingredients.Ingredients = new Dictionary<IngredientType, Ingredient>(shaurma.Ingredients);

            position = new Vector3(position.x, kebabPosition.y, position.z);

            var dir = (position - kebabPosition).normalized;

            kebabProjectile.OnSuccess += OnSuccess;
            kebabProjectile.TweenHandle = kebabProjectile.transform
                .DOMove(dir * DistanceToFly, DistanceToFly / _kebabSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() => Destroy(kebabProjectile.gameObject));
        }

        private void OnSuccess(KebabProjectile projectile, int score)
        {
            projectile.OnSuccess -= OnSuccess;
            GameManager.Instance.Score.Value += score;
        }

        public void SetIngredient(IngredientType ingredientType, Ingredient ingredient)
        {
            shaurma.SetIngredient(ingredientType, ingredient);
        }
    }
}