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
            int minId = type == IngredientType.Lavash ? 0 : -1;
            var randomId = Random.Range(minId, ingredients[type].Count);
            if (randomId == -1)
            {
                return null;
            }

            return ingredients[type][randomId];
        }
    }
}