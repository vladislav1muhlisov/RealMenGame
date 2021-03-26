using System.Collections.Generic;
using System.Linq;
using RealMenGame.Scripts.Bandits;
using RealMenGame.Scripts.Common;
using RealMenGame.Scripts.Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RealMenGame.Scripts
{
    public class SpawnManager : MonoBehaviourSingleton<SpawnManager>
    {
        private const int POOL_SIZE = 10;
        
        public LevelSettings LevelSettings;
        
        public Transform[] SpawnPoints;
        public Transform[] AwayPoints;

        private double _elapsedTime;
        private BanditPool[] _pools;

        private Transform GetRandomSpawnPoint()
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
                _pools[banditTypeIndex] = new BanditPool(LevelSettings.PossibleBandits[banditTypeIndex].Prefab, POOL_SIZE);
            }
        }

        private int GetRandomBandit()
        {
            var totalChanceValue = LevelSettings.SpawnChances.Sum();
            var value = Random.Range(0f, totalChanceValue);

            var min = 0f;

            for (var i = 0; i < LevelSettings.SpawnChances.Length; ++i)
            {
                var max = min + LevelSettings.SpawnChances[i];
                min += LevelSettings.SpawnChances[i];

                if (value < min || value > max) continue;

                return i;
            }

            return 0;
        }

        private void Update()
        {
            if (_elapsedTime >= 1f / LevelSettings.SpawnSpeed)
            {
                _elapsedTime -= 1f / LevelSettings.SpawnSpeed;

                var banditIndex = GetRandomBandit();
                var bandit = _pools[banditIndex].Spawn();

                bandit.transform.position = GetRandomSpawnPoint().position;
                bandit.gameObject.SetActive(true);
                bandit.BanditIndex = banditIndex;
                bandit.CurrentState = BanditController.BanditState.Spawned;
                bandit.Ingredients.Ingredients = new Dictionary<IngredientType, Ingredient>();
                
                var lavash = IngredientsConfigLoader.Instance.IngredientsConfig.LavashSprites;
                var meat = IngredientsConfigLoader.Instance.IngredientsConfig.MeatSprites;
                var sauce = IngredientsConfigLoader.Instance.IngredientsConfig.SauceSprites;
                var vegetables = IngredientsConfigLoader.Instance.IngredientsConfig.VegetablesSprites;

                bandit.Ingredients.Ingredients[IngredientType.Lavash] = lavash[Random.Range(0, lavash.Count)];
                bandit.Ingredients.Ingredients[IngredientType.Meat] = meat[Random.Range(0, meat.Count)];
                bandit.Ingredients.Ingredients[IngredientType.Sauce] = sauce[Random.Range(0, sauce.Count)];
                bandit.Ingredients.Ingredients[IngredientType.Vegetables] = vegetables[Random.Range(0, vegetables.Count)];
            }

            _elapsedTime += Time.deltaTime;
        }
    }
}