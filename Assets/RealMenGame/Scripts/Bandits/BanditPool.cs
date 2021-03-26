using System;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace RealMenGame.Scripts.Bandits
{
    public class BanditPool : IDisposable
    {
        private readonly ObjectPool<BanditController> _pool;

        public BanditPool(BanditController prefab, int maxSize = 50)
        {
            _pool = new ObjectPool<BanditController>(() => Object.Instantiate(prefab), actionOnDestroy: bandit => Object.Destroy(bandit.gameObject), maxSize: maxSize);
        }

        public BanditController Spawn()
        {
            return _pool.Get();
        }

        public void DeSpawn(BanditController bandit)
        {
            _pool.Release(bandit);
        }

        public void Dispose()
        {
            _pool?.Dispose();
        }
    }
}