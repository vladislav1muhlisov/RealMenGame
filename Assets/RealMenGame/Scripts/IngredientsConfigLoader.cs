using System.Collections.Generic;
using System.Linq;
using RealMenGame.Scripts.Common;

namespace RealMenGame.Scripts
{
    public class IngredientsConfigLoader : MonoBehaviourSingleton<IngredientsConfigLoader>
    {
        public IngredientsConfig IngredientsConfig;
        public Dictionary<IngredientType, Dictionary<int, Ingredient>> Ingredients;

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Ingredients = new Dictionary<IngredientType, Dictionary<int, Ingredient>>
            {
                {IngredientType.Lavash, IngredientsConfig.LavashSprites.ToDictionary(ingredient => ingredient.Id)},
                {IngredientType.Vegetables, IngredientsConfig.VegetablesSprites.ToDictionary(ingredient => ingredient.Id)},
                {IngredientType.Meat, IngredientsConfig.MeatSprites.ToDictionary(ingredient => ingredient.Id)},
                {IngredientType.Sauce, IngredientsConfig.SauceSprites.ToDictionary(ingredient => ingredient.Id)}
            };
        }
    }
}