using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RealMenGame.Scripts.UI
{
    public class IngredientsSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const float AnimationDuration = 0.3f;
        [SerializeField] private IngredientType ingredientType;
        [SerializeField] private List<Image> slots;
        [SerializeField] private List<Button> buttons;
        [SerializeField] private Button fakeButton;
        [SerializeField] private Image fakeButtonImage;
        private List<Ingredient> _ingredients;
        private bool _isRotating;

        private readonly List<Ingredient> _ingredientsSlots = new List<Ingredient>
        {
            null, null, null
        };

        private int _currentLowSlotIngredientIndex;
        private bool _isPointerEnter;
        private readonly Vector3 _smallScale = new Vector3(0.8f, 0.8f, 0.8f);

        private void Awake()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                var i1 = i;
                buttons[i].onClick.AddListener(() => OnSlotClick(i1));
            }

            buttons[0].transform.localScale = _smallScale;
            buttons[1].transform.localScale = Vector3.one;
            buttons[2].transform.localScale = _smallScale;

            _ingredients = IngredientsConfigLoader.Instance.Ingredients[ingredientType].Values
                .OrderBy(ingredient => ingredient.Id).ToList();
            if (ingredientType != IngredientType.Lavash)
            {
                _ingredients.Insert(0, null);
            }

            _currentLowSlotIngredientIndex = _ingredients.Count - 1;
            UpdateSlots();
        }

        private void Update()
        {
            if (_isPointerEnter)
            {
                if (!_isRotating)
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
        }

        private void OnDestroy()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].onClick.RemoveAllListeners();
            }
        }

        private void OnSlotClick(int index)
        {
            if (_isRotating)
            {
                return;
            }

            if (index == 0)
            {
                RotateUp().Forget();
            }
            else if (index == 2)
            {
                RotateDown().Forget();
            }
        }

        private void UpdateSlots()
        {
            for (int i = 0; i < 3; i++)
            {
                Ingredient ingredient = _ingredients[(_currentLowSlotIngredientIndex + i) % _ingredients.Count];
                _ingredientsSlots[i] = ingredient;
                if (ingredient != null)
                {
                    slots[i].gameObject.SetActive(true);
                    slots[i].sprite = ingredient.Sprite;
                }
                else
                {
                    slots[i].gameObject.SetActive(false);
                }
            }

            var selectedIngredient = _ingredientsSlots[1];
            StallManager.Instance.SetIngredient(ingredientType, selectedIngredient);
        }

        private async UniTask RotateUp()
        {
            _isRotating = true;
            _currentLowSlotIngredientIndex++;
            _currentLowSlotIngredientIndex %= _ingredients.Count;
            await RotateAsync(1);
            _isRotating = false;
        }

        private async UniTask RotateDown()
        {
            _isRotating = true;
            _currentLowSlotIngredientIndex--;
            if (_currentLowSlotIngredientIndex < 0)
            {
                _currentLowSlotIngredientIndex = _ingredients.Count - 1;
            }

            await RotateAsync(-1);
            _isRotating = false;
        }

        private async UniTask RotateAsync(int delta)
        {
            var centralPosition = buttons[1].transform.localPosition;
            var deltaPosition = buttons[0].transform.localPosition - centralPosition;
            var deltaPositionSigned = deltaPosition * delta;

            var fakeButtonTransform = fakeButton.transform;
            fakeButtonTransform.localPosition = centralPosition - deltaPositionSigned * 2;
            fakeButtonTransform.localScale = _smallScale;

            SetFakeButtonSprite(delta);

            var moving = UniTask.WhenAll(
                buttons[0].transform
                    .DOLocalMove(centralPosition + deltaPosition + deltaPositionSigned, AnimationDuration)
                    .ToUniTask(),
                buttons[1].transform.DOLocalMove(centralPosition + deltaPositionSigned,
                        AnimationDuration)
                    .ToUniTask(),
                buttons[2].transform
                    .DOLocalMove(centralPosition - deltaPosition + deltaPositionSigned,
                        AnimationDuration)
                    .ToUniTask());

            var scaling = UniTask.WhenAll(
                buttons[0].transform.DOScale(delta < 0 ? Vector3.one : _smallScale, AnimationDuration).ToUniTask(),
                buttons[1].transform.DOScale(_smallScale, AnimationDuration).ToUniTask(),
                buttons[2].transform.DOScale(delta > 0 ? Vector3.one : _smallScale, AnimationDuration).ToUniTask(),
                fakeButtonTransform.DOScale(Vector3.one, AnimationDuration).ToUniTask());

            var fakeButtonMoving =
                fakeButtonTransform.DOLocalMove(centralPosition - deltaPositionSigned, AnimationDuration)
                    .ToUniTask();

            await UniTask.WhenAll(moving, scaling, fakeButtonMoving);

            buttons[0].transform.localScale = _smallScale;
            buttons[1].transform.localScale = Vector3.one;
            buttons[2].transform.localScale = _smallScale;

            buttons[0].transform.localPosition = centralPosition + deltaPosition;
            buttons[1].transform.localPosition = centralPosition;
            buttons[2].transform.localPosition = centralPosition - deltaPosition;

            fakeButtonTransform.localPosition = centralPosition + deltaPositionSigned * 2;

            UpdateSlots();
        }

        private void SetFakeButtonSprite(int delta)
        {
            int indexForLowNextSprite = _currentLowSlotIngredientIndex - 1;
            if (indexForLowNextSprite < 0)
            {
                indexForLowNextSprite = _ingredients.Count - 1;
            }

            int indexForUpperNextImage = (_currentLowSlotIngredientIndex + 2) % _ingredients.Count;

            var ingredient = delta > 0 ? _ingredients[indexForUpperNextImage] : _ingredients[indexForLowNextSprite];
            if (ingredient == null)
            {
                fakeButtonImage.gameObject.SetActive(false);
            }
            else
            {
                fakeButtonImage.gameObject.SetActive(true);
                fakeButtonImage.sprite = ingredient.Sprite;
            }
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