using UnityEngine;
using Core;
using System.Collections.Generic;

namespace InGame
{
    public class Level : MonoBehaviour
    {
        private Dictionary<string, InGameService> _services;
        private ServiceManager _service;
        private DataManager _dataManager;
        private PoolManager _poolManager;

        private void Awake()
        {
            _service = ServiceManager.Instance;
            var gameManager = _service.GetCoreManager<GameManager>();
            gameManager.SetLevel(this);
            Initialize();
        }

        private void OnDestroy()
        {
            CleanupLevel();
        }

        private void OnDisable()
        {
            CleanupLevel();
        }

        public void Initialize()
        {
            _dataManager = _service.GetCoreManager<DataManager>();
            _poolManager = _service.GetCoreManager<PoolManager>();
            _services = new Dictionary<string, InGameService>();
        }

        private void CleanupLevel()
        {
            if (_poolManager != null)
            {
                _poolManager.Clear();
            }
        }

        public void Register<T>(string name, T service) where T : InGameService
        {
            if (_services.ContainsKey(name))
            {
                Debug.LogError($"already exist. -> {GetType().Name}.");
                return;
            }
            _services.Add(name, service);
        }

        public T Get<T>() where T : InGameService
        {
            string key = typeof(T).Name;
            if (!_services.ContainsKey(key))
            {
                Debug.LogError($"not registered" + " key = " + key);
                return null;
            }
            return (T)_services[key];
        }

        public bool ContainsKey<T>()
        {
            string key = typeof(T).Name;
            return _services.ContainsKey(key);
        }
    }
}