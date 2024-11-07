using System;
using System.Collections;
using UnityEngine;
using Core;
using ColorBlockData;
using ColorBlocks;

namespace InGame
{
    public class GridManager : InGameService
    {
        [Header("Grid Settings")]
        [SerializeField] private float _cellSize;
        [SerializeField] private float _gridHeight = 0f;
        [SerializeField] private Transform _gridParent;

        [Header("Visual Settings")]
        [SerializeField] private Material _cellMaterial;

        private ColorBlockInfoManager _levelManager;
        private GridCellPool _cellPool;
        private LevelData _levelData;
        private GridCell[,] _grid;

        public override IEnumerator Initialize()
        {

            _levelManager = _gameManager.GetInGameManager<ColorBlockInfoManager>();
            _cellPool = _serviceManager.GetCoreManager<PoolManager>().GetPool<GridCellPool>();
            _levelData = _levelManager.GetCurrentLevelData();

            if (_levelData == null)
            {
                yield break;
            }

            CreateGrid();
            CreateCells();

            yield return _waitForEndFrame;
        }

        private void CreateGrid()
        {
            _grid = new GridCell[_levelData.RowCount, _levelData.ColCount];
        }

        private void CreateCells()
        {
            foreach (var cellInfo in _levelData.CellInfo)
            {
                var worldPos = GetWorldPosition(cellInfo.Row, cellInfo.Col);

                var cellController = _cellPool.GetFromPool();
                cellController.Active();

                var cellTransform = cellController.GetTransform();
                cellTransform.position = worldPos;
                cellTransform.localScale = new Vector3(_cellSize, 0.1f, _cellSize);

                cellController.SetMaterial(_cellMaterial);
                cellController.SetGridPosition(new Vector2Int(cellInfo.Row, cellInfo.Col));

                _grid[cellInfo.Row, cellInfo.Col] = cellController;
            }
        }

        private Vector3 GetWorldPosition(int row, int col)
        {
            float x = col * _cellSize;
            float z = -row * _cellSize;
            Vector3 position = _gridParent.position + new Vector3(x, _gridHeight, z);
            return position;
        }

        private void OnDisable()
        {
            if (_grid == null) return;

            for (int row = 0; row < _levelData.RowCount; row++)
            {
                for (int col = 0; col < _levelData.ColCount; col++)
                {
                    var cell = _grid[row, col];
                    if (cell != null)
                    {
                        _cellPool.PutToPool(cell);
                    }
                }
            }
        }
    }
}