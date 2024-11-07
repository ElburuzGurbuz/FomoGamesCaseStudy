using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-100_099)]
    public abstract class Service : MonoBehaviour
    {
        protected WaitForEndOfFrame _waitForEndOfFrame;
        //protected bool _isReady = false;
        protected Dictionary<string, Service> _dependencys;
        protected ServiceManager _serviceManager;

        public abstract IEnumerator Initialize();

        public virtual void Awake()
        {
            _waitForEndOfFrame = new WaitForEndOfFrame();
            _dependencys = new Dictionary<string, Service>();

            _serviceManager = ServiceManager.Instance;

            _serviceManager.RegisterCoreManager(this);
        }
    }
}