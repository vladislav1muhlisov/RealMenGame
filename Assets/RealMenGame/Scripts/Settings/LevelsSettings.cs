using UnityEngine;

namespace RealMenGame.Scripts.Settings
{
    [CreateAssetMenu(fileName = "LevelsSettings", menuName = "RealMen/LevelsSettings", order = 0)]
    public class LevelsSettings : ScriptableObject
    {
        public LevelSettings[] Levels;
    }
}