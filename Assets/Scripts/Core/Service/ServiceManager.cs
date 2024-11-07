using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-100_100)]
    public class ServiceManager : MonoBehaviour
    {
        public static ServiceManager Instance;
        private Dictionary<string, Service> _services;

        private void Awake()
        {
            Instance = this;
            _services = new Dictionary<string, Service>();
        }
        public void RegisterCoreManager(Service service)
        {
            var key = service.GetType().Name;
            if (_services.ContainsKey(key))
            {
                Debug.LogError($"failed to register");
                return;
            }
            _services.Add(key, service);
        }
        public T GetCoreManager<T>() where T : Service
        {
            var name = typeof(T).Name;
            if (!_services.ContainsKey(name))
            {
                Debug.LogError($"key not found: '{name}'");
                return null;
            }
            return (T)_services[name];
        }
    }
}

