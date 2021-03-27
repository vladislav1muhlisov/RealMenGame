using RealMenGame.Scripts.Bandits;
using UnityEngine;

namespace RealMenGame.Scripts.Settings
{
    [CreateAssetMenu(fileName = "BanditSettings", menuName = "RealMen/BanditSettings", order = 0)]
    public class BanditSettings : ScriptableObject
    {
        public BanditController Prefab;
        public int Damage = 10;
        public int Score = 10;
        
        public float AnimationRightDelay = 3.5f;
        public float AnimationWrongDelay = 6.333f;

        public float AwaySpeed = 0.8f;
        public float NormalSpeed = 0.4f;
    }
}