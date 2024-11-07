using System.Collections;
using UnityEngine;
using Core;
using ColorBlocks;
using ColorBlockData;

namespace InGame
{
    public class ExitBlockManager : InGameService
    {
        [Header("Grid Settings")]
        [SerializeField] private float _cellSize = 2f;
        [SerializeField] private float _gridHeight = 0f;
        [SerializeField] private Transform _gridParent;

        private LevelData _levelData;
        private ExitBlockPool _exitBlockPool;
        private ColorBlockInfoManager _levelManager;
        private InGameEventSystem _eventSystem;

        public override IEnumerator Initialize()
        {

            _levelManager = _gameManager.GetInGameManager<ColorBlockInfoManager>();
            _exitBlockPool = _serviceManager.GetCoreManager<PoolManager>().GetPool<ExitBlockPool>();
            _eventSystem = _gameManager.GetInGameManager<InGameEventSystem>();
            _levelData = _levelManager.GetCurrentLevelData();

            // Event'lere cleanup metodunu baðla
            _eventSystem.AddManuelWinEvent(CleanupExitBlocks);
            _eventSystem.AddManuelFailEvent(CleanupExitBlocks);

            if (_levelData == null)
            {
                yield break;
            }

            CreateExitBlocks();

            yield return _waitForEndFrame;
        }

        private void CleanupExitBlocks()
        {

            // Sahnedeki tüm aktif ExitBlock'larý bul
            var exitBlocks = FindObjectsOfType<ExitBlock>();

            foreach (var exitBlock in exitBlocks)
            {
                if (exitBlock.gameObject.activeSelf)
                {
                    exitBlock.Inactive();
                    _exitBlockPool.PutToPool(exitBlock);
                }
            }

        }

        private void OnDestroy()
        {
            if (_eventSystem != null)
            {
                _eventSystem.RemoveManuelWinEvent(CleanupExitBlocks);
                _eventSystem.RemoveManuelFailEvent(CleanupExitBlocks);
            }
        }

        // Mevcut CreateExitBlocks ve diðer metodlar ayný kalacak...
        private void CreateExitBlocks()
        {
            if (_levelData == null || _levelData.ExitInfo == null)
            {
                return;
            }

            foreach (var exitInfo in _levelData.ExitInfo)
            {
                ExitBlock matchedExit = null;
                bool foundMatch = false;

                while (!foundMatch)
                {
                    ExitBlock exit = _exitBlockPool.GetFromPool();

                    if (exit == null)
                    {
                        break;
                    }

                    if (exit.GetColorId() == exitInfo.Colors &&
                        exit.GetDirection() == exitInfo.Direction)
                    {
                        matchedExit = exit;
                        foundMatch = true;
                    }
                    else
                    {
                        _exitBlockPool.PutToPool(exit);
                    }
                }

                if (matchedExit != null)
                {
                    Vector3 worldPos = GetWorldPosition(exitInfo.Row, exitInfo.Col, exitInfo.Direction);
                    Transform exitTransform = matchedExit.GetTransform();
                    exitTransform.position = worldPos;

                    float yRotation = exitInfo.Direction * 90f;
                    exitTransform.rotation = Quaternion.Euler(0, yRotation, 0);

                    matchedExit.Setup(new Vector2Int(exitInfo.Row, exitInfo.Col));
                    matchedExit.Active();
                }
            }
        }

        private Vector3 GetWorldPosition(int row, int col, int direction)
        {
            float centerX = col * _cellSize;
            float centerZ = -row * _cellSize;

            Vector3 offset = Vector3.zero;
            switch (direction)
            {
                case 0: // Up
                    offset = new Vector3(0, 0, _cellSize / 2);
                    break;
                case 1: // Right
                    offset = new Vector3(_cellSize / 2, 0, 0);
                    break;
                case 2: // Down
                    offset = new Vector3(0, 0, -_cellSize / 2);
                    break;
                case 3: // Left
                    offset = new Vector3(-_cellSize / 2, 0, 0);
                    break;
            }

            Vector3 position = _gridParent.position + new Vector3(centerX, _gridHeight, centerZ) + offset;
            return position;
        }
    }
}