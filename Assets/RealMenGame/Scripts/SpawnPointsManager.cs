using RealMenGame.Scripts.Common;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class SpawnPointsManager : MonoBehaviourSingleton<SpawnPointsManager>
    {
        public Transform[] SpawnPoints;
        public Transform[] AwayPoints;

        public Transform GetRandomSpawnPoint()
        {
            return SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        }

        public Transform GetRandomAwayPoint()
        {
            return AwayPoints[Random.Range(0, AwayPoints.Length)];
        }
    }
}