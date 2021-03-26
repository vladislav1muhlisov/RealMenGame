using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class Shaurma : MonoBehaviour
    {
        [SerializeField] private Transform defaultLocation;

        public List<IngredientType> Ingredients;

        public void AddIngredient(IngredientType ingredientType)
        {
            Ingredients.Add(ingredientType);
        }

        public async UniTask FlyTo(Transform target)
        {
            await transform.DOMove(target.position, 1f).AsyncWaitForCompletion();
        }

        public void ResetShaurma()
        {
            transform.localPosition = Vector3.zero;
        }
    }
}