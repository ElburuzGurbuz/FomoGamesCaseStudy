using ColorBlockData;
using ColorBlocks;
using Core;
using InGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InGame
{
    public class MoveableBlockManager : InGameService
    {
        [Header("Grid Settings")]
        [SerializeField] private float _cellSize;
        [SerializeField] private float _gridHeight = 0f;
        [SerializeField] private Transform _gridParent;

        private LevelData _levelData;
        private BlockTypesPool _colorBlockPool;
        private ColorBlockInfoManager _levelManager;

        public override IEnumerator Initialize()
        {

            _levelManager = _gameManager.GetInGameManager<ColorBlockInfoManager>();
            _colorBlockPool = _serviceManager.GetCoreManager<PoolManager>().GetPool<BlockTypesPool>();
            _levelData = _levelManager.GetCurrentLevelData();

            if (_levelData == null)
            {
                yield break;
            }

            CreateColorBlocks();

            yield return _waitForEndFrame;
        }
        private void CreateColorBlocks()
        {
            if (_levelData == null || _levelData.MovableInfo == null)
            {
                return;
            }

            // Her bir MovableInfo için pooldan uygun bloðu bul ve aktifleþtir
            foreach (var blockInfo in _levelData.MovableInfo)
            {
                BlockOrientation orientation = DetermineOrientation(blockInfo.Direction);
                Block matchedBlock = null;
                bool foundMatch = false;

                // Kriterlere uyan bloðu bulana kadar pool'dan blok almaya devam et
                while (!foundMatch)
                {
                    Block block = _colorBlockPool.GetFromPool();

                    if (block == null)
                    {
                        break;
                    }

                    // Block özelliklerini kontrol et
                    if (block.GetColorId() == blockInfo.Colors &&
                        block.GetLength() == blockInfo.Length &&
                        block.GetOrientation() == orientation)
                    {
                        matchedBlock = block;
                        foundMatch = true;
                    }
                    else
                    {
                        // Eþleþmeyen bloðu pool'a geri koy
                        _colorBlockPool.PutToPool(block);
                    }
                }

                if (matchedBlock != null)
                {
                    // Dünya pozisyonunu hesapla ve bloðu yerleþtir
                    Vector3 worldPos = GetWorldPosition(blockInfo.Row, blockInfo.Col);
                    Transform blockTransform = matchedBlock.GetTransform();
                    blockTransform.position = new Vector3(worldPos.x, _gridHeight, worldPos.z);

                    // Bloðu konfigüre et ve aktifleþtir
                    matchedBlock.Setup(
                        gridPosition: new Vector2Int(blockInfo.Row, blockInfo.Col),
                        allowedDirections: blockInfo.Direction
                    );

                    matchedBlock.Active();

                }
            }
        }
        private Vector3 GetWorldPosition(int row, int col)
        {
            float x = col * _cellSize;
            float z = -row * _cellSize;
            Vector3 position = _gridParent.position + new Vector3(x, _gridHeight, z);
            return position;
        }

        private BlockOrientation DetermineOrientation(int[] directions)
        {
            // 0 (Up) ve 2 (Down) -> Vertical
            // 1 (Right) ve 3 (Left) -> Horizontal
            bool hasVertical = Array.Exists(directions, d => d == 0 || d == 2);
            bool hasHorizontal = Array.Exists(directions, d => d == 1 || d == 3);

            if (hasVertical && !hasHorizontal)
                return BlockOrientation.Vertical;

            return BlockOrientation.Horizontal; // Varsayýlan olarak horizontal
        }
    }
}