using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System.Diagnostics;
using ColorBlocks;

namespace InGame
{
    public class InGameLoaderManager : InGameService
    {
        private Stopwatch _initialTime;
        private bool _inGameLoadingFinish = false;
        public override IEnumerator Initialize()
        {
            _initialTime = new Stopwatch();
            _initialTime.Start();

            var service=ServiceManager.Instance;
            var gameManager = service.GetCoreManager<GameManager>();
            Debug2.LogEditor("loader has been started");

            var inGameSaveSystem=gameManager.GetInGameManager<InGameSaveSystemManager>();
            yield return StartCoroutine(inGameSaveSystem.Initialize());
            Debug2.LogEditor("InGameSaveSystem has been started");

            var inGameCameraManager = gameManager.GetInGameManager<InGameCameraManagers>();
            yield return StartCoroutine(inGameCameraManager.Initialize());
            Debug2.LogEditor("InGameCameraManagers has been started");

            var inGameEventSystem = gameManager.GetInGameManager<InGameEventSystem>();
            yield return StartCoroutine(inGameEventSystem.Initialize());
            Debug2.LogEditor("InGameEventSystem has been started");

            var inGameMoneyManager = gameManager.GetInGameManager<InGameMoneyManager>();
            yield return StartCoroutine(inGameMoneyManager.Initialize());
            Debug2.LogEditor("InGameMoneyManager has been started");

            var inGameTouchManager = gameManager.GetInGameManager<InGameTouchManager>();
            yield return StartCoroutine(inGameTouchManager.Initialize());
            Debug2.LogEditor("InGameTouchManager has been started");

            var ColorBlockInfoManager = gameManager.GetInGameManager<ColorBlockInfoManager>();
            yield return StartCoroutine(ColorBlockInfoManager.Initialize());
            Debug2.LogEditor("colorBlockLevelManager has been started");

            var GridManager = gameManager.GetInGameManager<GridManager>();
            yield return StartCoroutine(GridManager.Initialize());
            Debug2.LogEditor("GridManager has been started");

            var MoveableBlockManager = gameManager.GetInGameManager<MoveableBlockManager>();
            yield return StartCoroutine(MoveableBlockManager.Initialize());
            Debug2.LogEditor("MoveableBlockManager has been started");

            var ExitBlockManager = gameManager.GetInGameManager<ExitBlockManager>();
            yield return StartCoroutine(ExitBlockManager.Initialize());
            Debug2.LogEditor("ExitBlockManager has been started");

            var GridStateManager = gameManager.GetInGameManager<GridStateManager>();
            yield return StartCoroutine(GridStateManager.Initialize());
            Debug2.LogEditor("GridStateManager has been started");

            var MoveLimitManager = gameManager.GetInGameManager<MoveLimitManager>();
            yield return StartCoroutine(MoveLimitManager.Initialize());
            Debug2.LogEditor("MoveLimitManager has been started");

            var inGameReadyToGo = gameManager.GetInGameManager<InGameReadyToPlayManager>();
            yield return StartCoroutine(inGameReadyToGo.Initialize());
            Debug2.LogEditor("InGameReadyToPlayManager has been started");

            _inGameLoadingFinish = true;

            _initialTime.Stop();
            Debug2.LogEditor($"InGame Managers Execution Time: {_initialTime.ElapsedMilliseconds} ms", ColorForDebug2.Orange);
#if !UNITY_EDITOR
            UnityEngine.Debug.Log($"InGame Managers Execution Time: {_initialTime.ElapsedMilliseconds} ms");
#endif
        }

        private void Start()
        {
            StartCoroutine(Initialize());
        }
        public bool GetInGameLoadingFinish()
        {
            return _inGameLoadingFinish;
        }
    }
}
