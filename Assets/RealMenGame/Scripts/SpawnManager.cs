﻿using System.Linq;
using RealMenGame.Scripts.Bandits;
using RealMenGame.Scripts.Common;
using RealMenGame.Scripts.Settings;
using UnityEngine;

namespace RealMenGame.Scripts
{
    public class SpawnManager : MonoBehaviourSceneSingleton<SpawnManager>
    {
        private const int POOL_SIZE = 10;

        public Transform Ground;

        public LevelSettings LevelSettings => GameManager.Instance._currentLevel;

        public SpawnPoint[] SpawnPoints;
        public Transform[] AwayPoints;

        private double _elapsedTime;
        private BanditPool[] _pools;

        private SpawnPoint GetRandomSpawnPoint()
        {
            return SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        }

        public Transform GetRandomAwayPoint()
        {
            return AwayPoints[Random.Range(0, AwayPoints.Length)];
        }

        public void DeSpawnBandit(BanditController bandit)
        {
            bandit.gameObject.SetActive(false);

            _pools[bandit.BanditIndex].DeSpawn(bandit);
        }

        protected override void OnSingletonAwake()
        {
            _elapsedTime = 0f;

            InitializePools();

            base.OnSingletonAwake();
        }

        private void InitializePools()
        {
            _pools = new BanditPool[LevelSettings.PossibleBandits.Length];

            for (var banditTypeIndex = 0; banditTypeIndex < _pools.Length; ++banditTypeIndex)
            {
                _pools[banditTypeIndex] =
                    new BanditPool(LevelSettings.PossibleBandits[banditTypeIndex].Prefab, POOL_SIZE);
            }
        }

        private int GetRandomBandit()
        {
            var totalChanceValue = LevelSettings.SpawnChances.Sum();
            var value = Random.Range(0f, totalChanceValue);

            var min = 0f;

            for (var i = 0; i < LevelSettings.SpawnChances.Length; ++i)
            {
                min += LevelSettings.SpawnChances[i];
                var max = min + LevelSettings.SpawnChances[i];

                if (value < min || value > max) continue;

                return i;
            }

            return 0;
        }

        private Vector3 PlaceOnGround(Vector3 position)
        {
            return new Vector3(position.x, Ground.position.y + 0.01f, position.z);
        }

        private void Update()
        {
            if (_elapsedTime >= 1f / LevelSettings.SpawnSpeed)
            {
                _elapsedTime -= 1f / LevelSettings.SpawnSpeed;

                var banditIndex = GetRandomBandit();
                var bandit = _pools[banditIndex].Spawn();

                var spawnPoint = GetRandomSpawnPoint();

                bandit.Settings = LevelSettings.PossibleBandits[banditIndex];
                bandit.transform.position = PlaceOnGround(spawnPoint.transform.position);

                bandit.WayPoints = spawnPoint.GetRandomWay().WayPoints.Select(p => PlaceOnGround(p.position)).ToArray();

                bandit.gameObject.SetActive(true);
                bandit.BanditIndex = banditIndex;
                bandit.CurrentState = BanditController.BanditState.Spawned;
            }

            _elapsedTime += Time.deltaTime;
        }
    }
}