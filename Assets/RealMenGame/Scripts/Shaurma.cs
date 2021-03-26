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

        public Dictionary<IngredientType, int> Ingredients = new Dictionary<IngredientType, int>();


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

        public void SetIngredient(IngredientType ingredientType, int number)
        {
            Ingredients[ingredientType] = number;
            if (number == -1)
            {
                Images[ingredientType].gameObject.SetActive(false);
            }
            else
            {
                Images[ingredientType].gameObject.SetActive(true);
                Images[ingredientType].sprite =
                    IngredientsConfigLoader.Instance.Ingredients[ingredientType][number].Sprite;
            }
        }

        public void FlyTo(Vector3 position)
        {
            canvas.SetActive(false);
            transform.DOMove(position, 0.2f);
        }

        public void ResetShaurma()
        {
            SetIngredient(IngredientType.Lavash, 0);
            SetIngredient(IngredientType.Meat, -1);
            SetIngredient(IngredientType.Vegetables, -1);
            SetIngredient(IngredientType.Sauce, -1);
            
            transform.localPosition = Vector3.zero;
            canvas.SetActive(true);
        }
    }
}