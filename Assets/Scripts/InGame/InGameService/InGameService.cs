using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace InGame
{
    public abstract class InGameService : MonoBehaviour
    {
        protected ServiceManager _serviceManager;
        protected GameManager _gameManager;
        protected DataManager _dataManager;

        protected WaitForEndOfFrame _waitForEndFrame;

        protected virtual void Awake()
        {
            _serviceManager = ServiceManager.Instance;
            _gameManager = _serviceManager.GetCoreManager<GameManager>();
            _dataManager = _serviceManager.GetCoreManager<DataManager>();

            _waitForEndFrame = new WaitForEndOfFrame();

            _gameManager.Register(GetType().Name, this);
            OnAwake();
        }
        public virtual void OnAwake()
        {
            Debug2.LogEditor($"this manager woke up \"{name}\"",ColorForDebug2.Aqua);
        }
        public abstract IEnumerator Initialize();
    }
}