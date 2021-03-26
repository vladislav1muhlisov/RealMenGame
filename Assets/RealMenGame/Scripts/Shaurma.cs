using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RealMenGame.Scripts
{
    public class Shaurma : MonoBehaviour
    {
        [SerializeField] private Image lavashImage;
        [SerializeField] private Image meatImage;
        [SerializeField] private Image vegetablesImage;
        [SerializeField] private Image sauceImage;

        [SerializeField] private GameObject canvas;

        public Dictionary<IngredientType, Ingredient> Ingredients = new Dictionary<IngredientType, Ingredient>();

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


        private void Awake()
        {
            ResetShaurma();
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

        public void ResetShaurma()
        {
            SetIngredient(IngredientType.Lavash,
                IngredientsConfigLoader.Instance.Ingredients[IngredientType.Lavash][0]);
            SetIngredient(IngredientType.Meat, null);
            SetIngredient(IngredientType.Vegetables, null);
            SetIngredient(IngredientType.Sauce, null);

            transform.localPosition = Vector3.zero;
            canvas.SetActive(true);
        }
    }
}