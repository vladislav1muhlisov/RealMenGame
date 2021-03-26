using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace RealMenGame.Scripts.UI
{
    public class TempIngredientButton : MonoBehaviour
    {
        [SerializeField] private IngredientType ingredientType;
        [SerializeField] private int ingredient;
        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AsObservable().Subscribe(u => OnClick());
        }

        public void OnClick()
        {
        }
    }
}