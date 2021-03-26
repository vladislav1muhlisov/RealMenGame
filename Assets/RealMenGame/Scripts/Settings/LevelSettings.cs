using UnityEngine;

namespace RealMenGame.Scripts.Settings
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "RealMen/Level", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        public BanditSettings[] PossibleBandits;
        public float[] SpawnChances;
        public float SpawnSpeed;
    }
}