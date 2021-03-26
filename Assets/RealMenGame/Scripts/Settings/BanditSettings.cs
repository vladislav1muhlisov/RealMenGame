using RealMenGame.Scripts.Bandits;
using UnityEngine;

namespace RealMenGame.Scripts.Settings
{
    [CreateAssetMenu(fileName = "BanditSettings", menuName = "RM/BanditSettings", order = 0)]
    public class BanditSettings : ScriptableObject
    {
        public BanditController Prefab;
    }
}