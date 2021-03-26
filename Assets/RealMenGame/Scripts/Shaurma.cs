using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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


        private Dictionary<IngredientType, Image> images;
        private Dictionary<IngredientType, List<Sprite>> sprites;

        private void Awake()
        {
            images = new Dictionary<IngredientType, Image>
            {
                {IngredientType.Lavash, lavashImage},
                {IngredientType.Meat, meatImage},
                {IngredientType.Vegetables, vegetablesImage},
                {IngredientType.Sauce, sauceImage},
            };
            var spritesConfig = SpritesConfig.Instance;

            sprites = new Dictionary<IngredientType, List<Sprite>>
            {
                {IngredientType.Lavash, spritesConfig.LavashSprites},
                {IngredientType.Meat, spritesConfig.MeatSprites},
                {IngredientType.Vegetables, spritesConfig.VegetablesSprites},
                {IngredientType.Sauce, spritesConfig.SauceSprites}
            };
            ResetShaurma();
        }

        public void SetIngredient(IngredientType ingredientType, int number)
        {
            Ingredients[ingredientType] = number;
            if (number == -1)
            {
                images[ingredientType].gameObject.SetActive(false);
            }
            else
            {
                images[ingredientType].gameObject.SetActive(true);
                images[ingredientType].sprite = sprites[ingredientType][number];
            }
        }

        public async UniTask FlyTo(Vector3 position)
        {
            canvas.SetActive(false);
            await transform.DOMove(position, 0.2f).AsyncWaitForCompletion();
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