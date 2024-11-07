using UnityEngine;
using System.Collections;
using System;
using Core;
using ColorBlockData;
using ColorBlocks;
using System.Collections.Generic;

namespace InGame
{
    public class GridStateManager : InGameService
    {
        private CellContent[,] _gridState;
        private int _rowCount;
        private int _colCount;
        private BlockTypesPool _blockTypesPool;

        public override IEnumerator Initialize()
        {

            var levelManager = _gameManager.GetInGameManager<ColorBlockInfoManager>();
            _blockTypesPool = _serviceManager.GetCoreManager<PoolManager>().GetPool<BlockTypesPool>();
            var levelData = levelManager.GetCurrentLevelData();

            _rowCount = levelData.RowCount;
            _colCount = levelData.ColCount;

            ResetGridState();

            PopulateBlocksFromLevel(levelData);
            PopulateExitsFromLevel(levelData);

            DebugPrintGrid();

            yield return _waitForEndFrame;
        }

        private void ResetGridState()
        {
            if (_gridState != null)
            {
                for (int row = 0; row < _rowCount; row++)
                {
                    for (int col = 0; col < _colCount; col++)
                    {
                        if (_gridState[row, col] != null)
                        {
                            _gridState[row, col].ClearExits();
                            _gridState[row, col].SetBlock(null);
                        }
                    }
                }
            }

            _gridState = new CellContent[_rowCount, _colCount];
            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    _gridState[row, col] = new CellContent();
                }
            }

        }

        public void Clear()
        {
            if (_gridState != null)
            {
                for (int row = 0; row < _rowCount; row++)
                {
                    for (int col = 0; col < _colCount; col++)
                    {
                        if (_gridState[row, col] != null)
                        {
                            var block = _gridState[row, col].Block;
                            if (block != null)
                            {
                                _blockTypesPool.PutToPool(block);
                                //block.Inactive();
                            }
                            _gridState[row, col].ClearExits();
                            _gridState[row, col].SetBlock(null);
                        }
                    }
                }
            }
        }

        private void OnDisable()
        {
            Clear();
        }
        private void PopulateBlocksFromLevel(LevelData levelData)
        {

            foreach (var blockInfo in levelData.MovableInfo)
            {
                Vector2Int position = new Vector2Int(blockInfo.Row, blockInfo.Col);
                BlockOrientation orientation = DetermineOrientation(blockInfo.Direction);

                if (orientation == BlockOrientation.Horizontal)
                {
                    for (int i = 0; i < blockInfo.Length; i++)
                    {
                        if (IsValidPosition(position.x, position.y + i))
                        {
                            var block = FindBlockAtPosition(position, blockInfo.Colors, blockInfo.Length);
                            if (block != null)
                            {
                                _gridState[position.x, position.y + i].SetBlock(block);
                            }
                        }
                    }
                }
                else // Vertical
                {
                    for (int i = 0; i < blockInfo.Length; i++)
                    {
                        if (IsValidPosition(position.x + i, position.y))
                        {
                            var block = FindBlockAtPosition(position, blockInfo.Colors, blockInfo.Length);
                            if (block != null)
                            {
                                _gridState[position.x + i, position.y].SetBlock(block);
                            }
                        }
                    }
                }
            }
        }
        public bool CheckRemainingBlocks()
        {
            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    var cell = _gridState[row, col];
                    if (cell.Block != null)
                    {
                        return true; // Hala blok var
                    }
                }
            }

            return false;
        }
        public void UpdateBlockPosition(Block block, Vector2Int oldPos, Vector2Int newPos)
        {
            if (block.GetOrientation() == BlockOrientation.Horizontal)
            {
                for (int i = 0; i < block.GetLength(); i++)
                {
                    if (IsValidPosition(oldPos.x, oldPos.y + i))
                    {
                        var cell = _gridState[oldPos.x, oldPos.y + i];
                        if (cell.Block == block)
                        {
                            cell.SetBlock(null);
                        }
                    }
                }
            }
            else // Vertical
            {
                for (int i = 0; i < block.GetLength(); i++)
                {
                    if (IsValidPosition(oldPos.x + i, oldPos.y))
                    {
                        var cell = _gridState[oldPos.x + i, oldPos.y];
                        if (cell.Block == block)
                        {
                            cell.SetBlock(null);
                        }
                    }
                }
            }

            if (block.GetOrientation() == BlockOrientation.Horizontal)
            {
                for (int i = 0; i < block.GetLength(); i++)
                {
                    if (IsValidPosition(newPos.x, newPos.y + i))
                    {
                        _gridState[newPos.x, newPos.y + i].SetBlock(block);
                    }
                }
            }
            else // Vertical
            {
                for (int i = 0; i < block.GetLength(); i++)
                {
                    if (IsValidPosition(newPos.x + i, newPos.y))
                    {
                        _gridState[newPos.x + i, newPos.y].SetBlock(block);
                    }
                }
            }

            DebugPrintGrid(); 
        }

        private void PopulateExitsFromLevel(LevelData levelData)
        {

            foreach (var exitInfo in levelData.ExitInfo)
            {
                Vector2Int position = new Vector2Int(exitInfo.Row, exitInfo.Col);
                if (IsValidPosition(position.x, position.y))
                {
                    var exit = FindExitAtPosition(position, exitInfo.Colors, exitInfo.Direction);
                    if (exit != null)
                    {
                        _gridState[position.x, position.y].SetExit(exit);
                    }
                }
            }
        }

        private Block FindBlockAtPosition(Vector2Int position, int colorId, int length)
        {
            Block[] allBlocks = FindObjectsOfType<Block>();

            foreach (var block in allBlocks)
            {
                if (block.GetColorId() == colorId &&
                    block.GetLength() == length &&
                    block.GetGridPosition() == position)
                {
                    return block;
                }
            }

            return null;
        }

        private ExitBlock FindExitAtPosition(Vector2Int position, int colorId, int direction)
        {
            ExitBlock[] allExits = FindObjectsOfType<ExitBlock>();

            foreach (var exit in allExits)
            {
                if (exit.GetColorId() == colorId &&
                    exit.GetDirection() == direction &&
                    exit.GetGridPosition() == position)
                {
                    return exit;
                }
            }

            return null;
        }

        private BlockOrientation DetermineOrientation(int[] directions)
        {
            bool hasVertical = Array.Exists(directions, d => d == 0 || d == 2);
            bool hasHorizontal = Array.Exists(directions, d => d == 1 || d == 3);

            if (hasVertical && !hasHorizontal)
                return BlockOrientation.Vertical;

            return BlockOrientation.Horizontal;
        }

        public bool IsValidPosition(int row, int col)
        {
            return row >= 0 && row < _rowCount && col >= 0 && col < _colCount;
        }

        public CellContent GetCellContent(int row, int col)
        {
            if (IsValidPosition(row, col))
                return _gridState[row, col];
            return null;
        }

        public void DebugPrintGrid()
        {
            string debugText = "\nGrid State:\n";
            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    var cell = _gridState[row, col];
                    string content = cell.Type switch
                    {
                        CellContentType.Empty => "[ ]",
                        CellContentType.Block => "[B]",
                        CellContentType.Exit => "[E]",
                        CellContentType.BlockAndExit => "[BE]",
                        _ => "[?]"
                    };
                    debugText += content;
                }
                debugText += "\n";
            }
        }

        public void ClearBlockFromGrid(Vector2Int pos, BlockOrientation orientation, int length)
        {
            if (orientation == BlockOrientation.Horizontal)
            {
                for (int i = 0; i < length; i++)
                {
                    if (IsValidPosition(pos.x, pos.y + i))
                    {
                        var cell = _gridState[pos.x, pos.y + i];
                        // Sadece bloðu temizle, gate'leri koru
                        cell.SetBlock(null);
                    }
                }
            }
            else // Vertical
            {
                for (int i = 0; i < length; i++)
                {
                    if (IsValidPosition(pos.x + i, pos.y))
                    {
                        var cell = _gridState[pos.x + i, pos.y];
                        // Sadece bloðu temizle, gate'leri koru
                        cell.SetBlock(null);
                    }
                }
            }
        }
    }
}