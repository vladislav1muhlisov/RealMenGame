using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RealMenGame.Scripts.UI
{
    public class ShaurmaDisplay : MonoBehaviour
    {
        [SerializeField] private Image lavashImage;
        [SerializeField] private Image meatImage;
        [SerializeField] private Image vegetablesImage;
        [SerializeField] private Image sauceImage;


        private Dictionary<IngredientType, Image> _images;

        private Dictionary<IngredientType, Image> Images
        {
            get
            {
                if (_images == null)
                {
                    _images = new Dictionary<IngredientType, Image>
                    {
                        {IngredientType.Lavash, lavashImage},
                        {IngredientType.Meat, meatImage},
                        {IngredientType.Vegetables, vegetablesImage},
                        {IngredientType.Sauce, sauceImage},
                    };
                }

                return _images;
            }
        }

        public void SetIngredients(Dictionary<IngredientType, Ingredient> ingredients)
        {
            foreach (var pair in ingredients)
            {
                SetIngredient(pair.Key, pair.Value);
            }
        }

        public void SetIngredient(IngredientType ingredientType, Ingredient ingredient)
        {
            if (ingredient == null)
            {
                Images[ingredientType].gameObject.SetActive(false);
            }
            else
            {
                Images[ingredientType].gameObject.SetActive(true);
                Images[ingredientType].sprite = ingredient.Sprite;
            }
        }
    }
}