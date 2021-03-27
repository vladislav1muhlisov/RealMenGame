using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RealMenGame.Scripts.Sounds;
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
        [SerializeField] private AudioClipSettings rotationSound;
        private List<Ingredient> _ingredients;
        private bool _isRotating;

        private static readonly Dictionary<IngredientType, (KeyCode up, KeyCode down)> inputKeys =
            new Dictionary<IngredientType, (KeyCode up, KeyCode down)>
            {
                {IngredientType.Lavash, (KeyCode.Q, KeyCode.A)},
                {IngredientType.Vegetables, (KeyCode.W, KeyCode.S)},
                {IngredientType.Meat, (KeyCode.E, KeyCode.D)},
                {IngredientType.Sauce, (KeyCode.R, KeyCode.F)}
            };

        private KeyCode _keyUp;
        private KeyCode _keyDown;

        private readonly List<Ingredient> _ingredientsSlots = new List<Ingredient>
        {
            null, null, null
        };

        private int _currentLowSlotIngredientIndex;
        private bool _isPointerEnter;
        private readonly Vector3 _smallScale = new Vector3(0.8f, 0.8f, 0.8f);

        private void Awake()
        {
            _keyUp = inputKeys[ingredientType].up;
            _keyDown = inputKeys[ingredientType].down;

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
            if (_isRotating)
            {
                return;
            }

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

            if (Input.GetKeyDown(_keyUp))
            {
                RotateUp().Forget();
            }

            if (Input.GetKeyDown(_keyDown))
            {
                RotateDown().Forget();
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
            UISound.Instance.Play(rotationSound);

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
                buttons[2].transform.DOScale(delta > 0 ? Vector3.one : _smallScale, AnimationDuration).ToUniTask());

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

            fakeButtonTransform.localScale = _smallScale;
            fakeButtonTransform.localPosition = centralPosition + deltaPositionSigned * 2;

            UpdateSlots();
        }

        private void SetFakeButtonSprite(int delta)
        {
            int indexForLowNextSprite = _currentLowSlotIngredientIndex - 1 - delta;
            if (indexForLowNextSprite < 0)
            {
                indexForLowNextSprite = _ingredients.Count - 1;
            }

            int indexForUpperNextImage = (_currentLowSlotIngredientIndex + 3 - delta) % _ingredients.Count;

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