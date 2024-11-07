using System.Collections;
using UnityEngine;
using Core;
using ColorBlocks;

namespace InGame
{
    public class MoveLimitManager : InGameService
    {
        private int _currentMoveLimit;
        private MoveLimitUIItem _moveLimitUI;
        private bool _hasLimit = true;
        private bool _isInitialized = false;
        private InGameEventSystem _eventSystem;

        public override IEnumerator Initialize()
        {
            var uiManager = _serviceManager.GetCoreManager<UIManager>();
            _moveLimitUI = uiManager.FindItem<CoreInGamePanel, MoveLimitUIItem>();
            _eventSystem = _gameManager.GetInGameManager<InGameEventSystem>();

            _eventSystem.AddManuelWinEvent(ResetMoveLimit);
            _eventSystem.AddManuelFailEvent(ResetMoveLimit);

            yield return _waitForEndFrame;

            var levelManager = _gameManager.GetInGameManager<ColorBlockInfoManager>();
            var levelData = levelManager.GetCurrentLevelData();

            if (levelData != null)
            {
                InitializeMoveLimit(levelData.MoveLimit);
            }
        }

        private void InitializeMoveLimit(int moveLimit)
        {
            _currentMoveLimit = moveLimit;

            if (_currentMoveLimit <= 0)
            {
                _hasLimit = false;
                if (_moveLimitUI != null)
                {
                    _moveLimitUI.CloseItem();
                }
            }
            else
            {
                _hasLimit = true;
                if (_moveLimitUI != null)
                {
                    _moveLimitUI.OpenItem();
                    _moveLimitUI.UpdateText(_currentMoveLimit);
                }
            }

            _isInitialized = true;
        }

        public void ResetMoveLimit()
        {

            _isInitialized = false;
            _hasLimit = true;
            _currentMoveLimit = 0;

            if (_moveLimitUI != null)
            {
                _moveLimitUI.CloseItem();
            }
        }

        public bool TryUseMove()
        {
            if (!_isInitialized)
            {
                return false;
            }

            if (!_hasLimit)
            {
                return true;
            }

            if (_currentMoveLimit <= 0)
            {
                return false;
            }

            _currentMoveLimit--;

            if (_moveLimitUI != null)
            {
                _moveLimitUI.UpdateText(_currentMoveLimit);
            }


            if (_currentMoveLimit <= 0)
            {
                _gameManager.Fail();
                return false;
            }

            return true;
        }

        public int GetRemainingMoves()
        {
            return _currentMoveLimit;
        }

        public bool HasMoveLimit()
        {
            return _hasLimit;
        }

        private void OnDestroy()
        {
            if (_eventSystem != null)
            {
                _eventSystem.RemoveManuelWinEvent(ResetMoveLimit);
                _eventSystem.RemoveManuelFailEvent(ResetMoveLimit);
            }
        }
    }
}