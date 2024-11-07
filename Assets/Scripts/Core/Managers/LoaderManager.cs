using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using InGame;

namespace Core
{
    public class LoaderManager : Service
    {
        private Stopwatch _initialTime;
        private TestPool _testPool;
        private void Start()
        {
            StartCoroutine(Initialize());
        }
        public override IEnumerator Initialize()
        {
            _initialTime = new Stopwatch();
            _initialTime.Start();

            var loadingManager = _serviceManager.GetCoreManager<LoadingManager>();
            yield return StartCoroutine(loadingManager.Initialize());
            Debug2.LogEditor("Loading Manager has been started");

            var dataManager = _serviceManager.GetCoreManager<DataManager>();
            yield return StartCoroutine(dataManager.Initialize());
            Debug2.LogEditor("Data Manager has been started");

            var uiManager = _serviceManager.GetCoreManager<UIManager>();
            yield return StartCoroutine(uiManager.Initialize());
            Debug2.LogEditor("UI Manager has been started");

            var soundManager = _serviceManager.GetCoreManager<SoundManager>();
            yield return StartCoroutine(soundManager.Initialize());
            Debug2.LogEditor("Sound Manager has been started");

            var poolManager = _serviceManager.GetCoreManager<PoolManager>();
            yield return StartCoroutine(poolManager.Initialize());
            Debug2.LogEditor("Pool Manager has been started");

            var touchManager = _serviceManager.GetCoreManager<TouchManager>();
            yield return StartCoroutine(touchManager.Initialize());
            Debug2.LogEditor("Touch Manager has been started");

            var gameManager = _serviceManager.GetCoreManager<GameManager>();
            yield return StartCoroutine(gameManager.Initialize());
            Debug2.LogEditor("Game Manager has been started");

            gameManager.CreateNewLevel(true);

            loadingManager.CloseLoadingScene();

            _initialTime.Stop();

#if !UNITY_EDITOR
            UnityEngine.Debug.Log($"Core Managers Execution Time: {_initialTime.ElapsedMilliseconds} ms");
#else
            Debug2.LogEditor($"Core Managers Execution Time: {_initialTime.ElapsedMilliseconds} ms", ColorForDebug2.Orange);
#endif
        }
    } 
}
    
