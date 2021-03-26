using RealMenGame.Scripts.Bandits;
using UnityEngine;

namespace RealMenGame.Scripts.Settings
{
    [CreateAssetMenu(fileName = "BanditSettings", menuName = "RealMen/BanditSettings", order = 0)]
    public class BanditSettings : ScriptableObject
    {
        public BanditController Prefab;
        public int Damage;
        
        public float AnimationRightDelay = 3.5f;
        public float AnimationWrongDelay = 6.333f;

        public float AwaySpeed;
        public float NormalSpeed;
    }
}