using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class PoolManager : Service
    {
        private readonly Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

        public override IEnumerator Initialize()
        {
            foreach (var item in _pools)
            {
                item.Value.Initialize();
            }
            yield return _waitForEndOfFrame;
        }
        public void Register(Type key, Pool s)
        {
            _pools.Add(key.Name, s);
        }
        public T GetPool<T>() where T : Pool
        {
            var name = typeof(T).Name;

            if (!_pools.ContainsKey(name))
            {
                Debug.LogError($"key not found for pool: '{name}'");
                return null;
            }

            return (T)_pools[name];
        }
        public void Clear()
        {
            foreach (var item in _pools)
            {
                item.Value.Clear();
            }
        }
    }
}
