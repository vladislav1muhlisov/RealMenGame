using System.Collections.Generic;
using RealMenGame.Scripts.Common;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class SpritesConfig : MonoBehaviourSingleton<SpritesConfig>
    {
        public List<Sprite> LavashSprites;
        public List<Sprite> VegetablesSprites;
        public List<Sprite> MeatSprites;
        public List<Sprite> SauceSprites;
    }
}