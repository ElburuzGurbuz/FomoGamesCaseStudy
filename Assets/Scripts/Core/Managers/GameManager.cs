using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Text;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using InGame;
using System;

namespace Core
{
    public partial class GameManager : Service
    {
        [ShowNonSerializedField]
        private const string AddressablePath = "Assets/Levels/";

        [SerializeField, HorizontalLine, Header("You must pust general game setting")]
        private GeneralGameSettings _generalGameSettings;

        private DataManager _dataManager;
        private UIManager _uIManager;

        private LevelIndicatorTextUIItem _levelIndicatorText;

        private GameObject _currentLevel;
        private GameObject _tempLevel;

        private int _levelData = 1;
        private bool _playable = true;
        private bool _pause = false;


        private StringBuilder _stringBuilderForAddressable;
        private AsyncOperationHandle<GameObject> _addressable;

        private Level _level = null;


        public override IEnumerator Initialize()
        {
            _dataManager = _serviceManager.GetCoreManager<DataManager>();
            _uIManager = _serviceManager.GetCoreManager<UIManager>();

            _levelIndicatorText = _uIManager.FindItem<CoreInGamePanel, LevelIndicatorTextUIItem>();

            _stringBuilderForAddressable = new StringBuilder();

            Application.targetFrameRate = _generalGameSettings.FrameRate;

            switch (_generalGameSettings.LevelCompletedStatus)
            {
                case LevelCompletedStatus.Loop:
                    LCLoop.Init(); break;

                case LevelCompletedStatus.FullRandomness:
                    LCFullRandomness.Init(); break;

                case LevelCompletedStatus.RegularRandomness:
                    LCRegularRandomness.Init(); break;
            }

            yield return _waitForEndOfFrame;
        }

        public void CreateNewLevel(bool initOrRetry = false)
        {
            _levelData = _dataManager.GetLevel();

            CheckLevelCompleted(_levelData);

            if (!initOrRetry)
                _levelData++;

            int levelNo = _levelData;

            var gameCompleted = _dataManager.GetWholeGameCompleted();
            if (gameCompleted)
                levelNo = LevelCompletedStatusHandle();
            else
                _dataManager.SetLevelName(levelNo);

            _dataManager.SetLevel(_levelData);

            //for ui indicator
            _levelIndicatorText.SetText(_levelData);

            if (_currentLevel != null)
            {
                // MoveLimitManager'ı bul ve sıfırla
                var moveLimitManager = GetInGameManager<MoveLimitManager>();
                if (moveLimitManager != null)
                {
                    moveLimitManager.ResetMoveLimit();
                }

                // GridStateManager'ı bul ve temizle
                var gridStateManager = GetInGameManager<GridStateManager>();
                if (gridStateManager != null)
                {
                    gridStateManager.Clear();
                }
            }

            StartCoroutine(LoadNewLevelAsync(levelNo));

        }


        private int LevelCompletedStatusHandle()
        {
            _dataManager.LevelStartEventClear();

            var levelNo = 0;
            switch (_generalGameSettings.LevelCompletedStatus)
            {
                case LevelCompletedStatus.Loop:
                    levelNo = LCLoop.Handle(_levelData);
                    break;

                case LevelCompletedStatus.FullRandomness:
                    levelNo = LCFullRandomness.Handle(_levelData);
                    break;

                case LevelCompletedStatus.RegularRandomness:
                    levelNo = LCRegularRandomness.Handle(_levelData);
                    break;
                case LevelCompletedStatus.LastLevel:
                    levelNo = _generalGameSettings.MaxLevel;
                    break;
            }
            _dataManager.SetLevelName(levelNo);
            return levelNo;
        }
        private void CheckLevelCompleted(int levelData)
        {
            int maxLevel = _generalGameSettings.MaxLevel;

            if (levelData == maxLevel)
                _dataManager.SetWholeGameCompleted(true);
        }
        private IEnumerator LoadNewLevelAsync(int levelNo)
        {
            _tempLevel = _currentLevel;
            CloseCurrentLevel();

            CallAddressable(levelNo);

            yield return null;
        }


        public void OpenCurrentLevel()
        {
            _currentLevel.SetActive(true);
        }
        private void CloseCurrentLevel()
        {
            if (_tempLevel == null)
                return;

            _tempLevel.SetActive(false);
        }
        public void CallAddressable(int levelNo)
        {
            _stringBuilderForAddressable.Clear();

            _stringBuilderForAddressable.Append(AddressablePath);
            _stringBuilderForAddressable.Append(levelNo);
            _stringBuilderForAddressable.Append(".prefab");

            var path = _stringBuilderForAddressable.ToString();
            _addressable = GetAddressable(path);

            _addressable.Completed -= LoadAssetAsyncCompleted;
            _addressable.Completed += LoadAssetAsyncCompleted;
        }
        private AsyncOperationHandle<GameObject> GetAddressable(string path)
        {
            return Addressables.InstantiateAsync(path);
        }
        private void LoadAssetAsyncCompleted(AsyncOperationHandle<GameObject> obj)
        {
            _currentLevel = obj.Result;

            OpenCurrentLevel();
        }
    }
    public partial class GameManager
    {
        // win and fail-----------------
        public void Win(bool withUI = true, int money = 0)
        {
            if (withUI)
            {
                WinState();
                _uIManager.ClosePanel<CoreInGamePanel>();
                _uIManager.FindPanel<WinPanel>().SetMoney(money);
                _uIManager.OpenPanel<WinPanel>();

            }
            else
            {
                GetInGameManager<InGameEventSystem>().CallWinEvents();
                CreateNewLevel();

                _uIManager.ClosePanel<WinPanel>();
                _uIManager.OpenPanel<CoreInGamePanel>();
            }
        }
        public void Fail(bool withUI = true)
        {
            if (withUI)
            {
                FailState();
                _uIManager.ClosePanel<CoreInGamePanel>();
                _uIManager.OpenPanel<FailPanel>();
            }
            else
            {
                GetInGameManager<InGameEventSystem>().CallFailEvents();
                CreateNewLevel(true);

                _uIManager.ClosePanel<FailPanel>();
                _uIManager.OpenPanel<CoreInGamePanel>();
            }
        }
        private void WinState()
        {
            SetPlayable(false);
        }
        private void FailState()
        {
            SetPlayable(false);
        }
        //------------------------------

        // pause and resume-------------
        public void Pause()
        {
            _pause = true;
            Time.timeScale = 0;
        }
        public void Resume()
        {
            _pause = false;
            Time.timeScale = 1;
        }
        //-----------------------------
        public bool GetPause()
        {
            return _pause;
        }
        public GeneralGameSettings GetSettings()
        {
            return _generalGameSettings;
        }

        public void SetPlayable(bool playable)
        {
            _playable = playable;
        }
        public bool GetPlayable()
        {
            return _playable;
        }
        //----------

        // In game service
        public void Register(string name, InGameService inGameService)
        {
            _level.Register(name, inGameService);
        }
        public T GetInGameManager<T>() where T : InGameService
        {
            return _level.Get<T>();
        }
        public void SetLevel(Level l)
        {
            _level = l;
        }
    }
}