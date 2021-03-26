using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace RealMenGame.Scripts.Bandits
{
    public static class IngredientsRandomGeneratorUtil
    {
        [MustUseReturnValue]
        public static Dictionary<IngredientType, Ingredient> Generate()
        {
            return new Dictionary<IngredientType, Ingredient>
            {
                {IngredientType.Lavash, GetRandomIngredient(IngredientType.Lavash)},
                {IngredientType.Vegetables, GetRandomIngredient(IngredientType.Vegetables)},
                {IngredientType.Meat, GetRandomIngredient(IngredientType.Meat)},
                {IngredientType.Sauce, GetRandomIngredient(IngredientType.Sauce)}
            };
        }

        private static Ingredient GetRandomIngredient(IngredientType type)
        {
            var ingredients = IngredientsConfigLoader.Instance.Ingredients;
            return ingredients[type][Random.Range(0, ingredients[type].Count)];
        }
    }
}