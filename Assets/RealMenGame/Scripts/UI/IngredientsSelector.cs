﻿using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RealMenGame.Scripts.UI
{
    public class IngredientsSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private IngredientType ingredientType;
        [SerializeField] private List<Image> slots;
        private List<Ingredient> _ingredients;

        private readonly List<Ingredient> _ingredientsSlots = new List<Ingredient>
        {
            null, null, null
        };

        private int _currentLowSlotIngredientId;
        private bool _isPointerEnter;

        private void Awake()
        {
            _ingredients = IngredientsConfigLoader.Instance.Ingredients[ingredientType].Values
                .OrderBy(ingredient => ingredient.Id).ToList();
            if (ingredientType != IngredientType.Lavash)
            {
                _ingredients.Insert(0, null);
            }

            _currentLowSlotIngredientId = 0;
            UpdateSlots();
        }

        private void Update()
        {
            if (_isPointerEnter)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    RotateUp().Forget();
                }
                else if (Input.mouseScrollDelta.y < 0)
                {
                    RotateDown().Forget();
                }
            }
        }

        private void UpdateSlots()
        {
            for (int i = 0; i < 3; i++)
            {
                Ingredient ingredient = _ingredients[(_currentLowSlotIngredientId + i) % _ingredients.Count];
                _ingredientsSlots[i] = ingredient;
                if (ingredient != null)
                {
                    slots[i].sprite = ingredient.Sprite;
                }
                else
                {
                    slots[i].sprite = null;
                }
            }

            var selectedIngredient = _ingredientsSlots[1];
            LarekManager.Instance.SetIngredient(ingredientType, selectedIngredient?.Id ?? -1);
        }

        private async UniTask RotateUp()
        {
            _currentLowSlotIngredientId++;
            _currentLowSlotIngredientId %= slots.Count;
            UpdateSlots();
        }

        private async UniTask RotateDown()
        {
            _currentLowSlotIngredientId--;
            if (_currentLowSlotIngredientId < 0)
            {
                _currentLowSlotIngredientId = _ingredients[_ingredients.Count - 1].Id;
            }

            UpdateSlots();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerEnter = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerEnter = false;
        }
    }
}