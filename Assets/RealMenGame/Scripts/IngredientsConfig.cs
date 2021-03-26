using System.Collections.Generic;
using UnityEngine;

namespace RealMenGame.Scripts
{
    [CreateAssetMenu(fileName = "IngredientsConfig", menuName = "RealMen/IngredientsConfig", order = 0)]
    public class IngredientsConfig : ScriptableObject
    {
        public List<Ingredient> LavashSprites;
        public List<Ingredient> VegetablesSprites;
        public List<Ingredient> MeatSprites;
        public List<Ingredient> SauceSprites;
    }
}