using System.Collections.Generic;
using RealMenGame.Scripts.UI;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class Shaurma : MonoBehaviour
    {
        [SerializeField] private ShaurmaDisplay shaurmaDisplay;

        public readonly Dictionary<IngredientType, Ingredient> Ingredients =
            new Dictionary<IngredientType, Ingredient>();


        private void Awake()
        {
            ResetShaurma();
        }

        public void SetIngredient(IngredientType ingredientType, Ingredient ingredient)
        {
            Ingredients[ingredientType] = ingredient;
            shaurmaDisplay.SetIngredient(ingredientType, ingredient);
        }

        private void ResetShaurma()
        {
            SetIngredient(IngredientType.Lavash,
                IngredientsConfigLoader.Instance.Ingredients[IngredientType.Lavash][0]);
            SetIngredient(IngredientType.Meat, null);
            SetIngredient(IngredientType.Vegetables, null);
            SetIngredient(IngredientType.Sauce, null);
        }
    }
}