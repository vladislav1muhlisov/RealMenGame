using System;
using DG.Tweening;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class KebabProjectile : MonoBehaviour
    {
        public KebabIngredients Ingredients;
        public Tween TweenHandle;
        public event Action<KebabProjectile, int> OnSuccess = delegate { };

        public void RaiseOnSuccess(int score)
        {
            OnSuccess(this, score);
        }
    }
}