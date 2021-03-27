using UnityEngine;

namespace RealMenGame.Scripts.Settings
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "RealMen/Level", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        public BanditSettings[] PossibleBandits;
        public float[] SpawnChances;
        public float SpawnSpeed = 0.1f;
        public int NextLevelScore = 200;
        public int MaxHealth = 100;
    }
}