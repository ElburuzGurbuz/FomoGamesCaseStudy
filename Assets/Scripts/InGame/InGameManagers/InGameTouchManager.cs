using UnityEngine;
using System.Collections;
using Core;
using DG.Tweening;

namespace InGame
{
    public class InGameTouchManager : InGameService
    {
        [Header("Swipe Settings")]
        [SerializeField] private float _minimumSwipeDistance = 50f;
        [SerializeField] private float _moveDuration = 0.3f;
        [SerializeField] private float _cellSize = 2f;
        [SerializeField] private float _disappearDuration = 0.3f;
        [SerializeField] private Transform _gridParent;

        private Vector3 _touchStart;
        private Block _selectedBlock;
        private GridStateManager _gridStateManager;
        private bool _isSwiping;
        private MoveLimitManager _moveLimitManager;
        private BlockTypesPool _blockTypesPool;
        public override IEnumerator Initialize()
        {
            _gridStateManager = _gameManager.GetInGameManager<GridStateManager>();
            _moveLimitManager = _gameManager.GetInGameManager<MoveLimitManager>();
            _blockTypesPool = _serviceManager.GetCoreManager<PoolManager>().GetPool<BlockTypesPool>();

            var touchManager = _serviceManager.GetCoreManager<TouchManager>();
            touchManager.Down -= Down;
            touchManager.Down += Down;
            touchManager.Up -= Up;
            touchManager.Up += Up;

            yield return _waitForEndFrame;
        }

        private void Down(TouchData touchData)
        {
            if (!touchData.HitCheck) return;

            _selectedBlock = touchData.Hit.transform.GetComponent<Block>();
            if (_selectedBlock != null)
            {
                _touchStart = touchData.ClickData;
                _isSwiping = true;
                
            }
        }

        private void Up(TouchData touchData)
        {
            if (!_isSwiping || _selectedBlock == null) return;

            Vector2 swipeDelta = touchData.ClickData - _touchStart;
            if (swipeDelta.magnitude < _minimumSwipeDistance)
            {
                ResetSwipe();
                return;
            }

            SwipeDirection direction = GetSwipeDirection(swipeDelta);

            if (IsValidMove(_selectedBlock, direction))
            {
                Vector2Int targetPos = FindFinalPosition(_selectedBlock, direction);


                // Ayný pozisyonda gate var mý kontrol et
                if (targetPos == _selectedBlock.GetGridPosition())
                {
                    var cell = _gridStateManager.GetCellContent(targetPos.x, targetPos.y);
                    if (cell != null && CheckGateMatch(cell, _selectedBlock, direction))
                    {
                        HandleDisappear(_selectedBlock, targetPos);
                    }
                }
                else
                {
                    MoveBlock(_selectedBlock, targetPos, direction);
                }
            }

            ResetSwipe();
        }
        private void HandleDisappear(Block block, Vector2Int position)
        {
            _gridStateManager.ClearBlockFromGrid(position, block.GetOrientation(), block.GetLength());
            //block.Inactive();
            _blockTypesPool.PutToPool(block);
            _gridStateManager.DebugPrintGrid();
        }

        private SwipeDirection GetSwipeDirection(Vector2 swipeDelta)
        {
            float angle = Mathf.Atan2(swipeDelta.y, swipeDelta.x) * Mathf.Rad2Deg;

            if (angle < 45 && angle > -45)
                return SwipeDirection.Right;
            if (angle < -135 || angle > 135)
                return SwipeDirection.Left;
            if (angle > 45 && angle < 135)
                return SwipeDirection.Up;
            return SwipeDirection.Down;
        }

        private bool IsValidMove(Block block, SwipeDirection direction)
        {
            BlockOrientation orientation = block.GetOrientation();
            Vector2Int currentPos = block.GetGridPosition();

            // Önce orientation kontrolü yapalým
            bool isValidDirection = orientation switch
            {
                BlockOrientation.Horizontal => direction == SwipeDirection.Left || direction == SwipeDirection.Right,
                BlockOrientation.Vertical => direction == SwipeDirection.Up || direction == SwipeDirection.Down,
                _ => false
            };

            if (!isValidDirection)
            {
                return false;
            }

            // Orientation kontrolü geçtiyse gate kontrolü yapalým
            var currentCell = _gridStateManager.GetCellContent(currentPos.x, currentPos.y);
            if (currentCell != null && currentCell.GetExits().Count > 0)
            {
                int requiredDirection = GetRequiredGateDirection(direction);
                foreach (var gate in currentCell.GetExits())
                {
                    if (gate.GetDirection() == requiredDirection && gate.GetColorId() == block.GetColorId())
                    {
                        return true;
                    }
                }
            }

            // Gate yoksa veya eþleþme yoksa ama yön uygunsa hareket edebilir
            return isValidDirection;
        }
        private Vector2Int FindFinalPosition(Block block, SwipeDirection direction)
        {
            Vector2Int currentPos = block.GetGridPosition();

            // Eðer mevcut konumda uygun gate varsa, direkt mevcut konumu döndür
            var currentCell = _gridStateManager.GetCellContent(currentPos.x, currentPos.y);
            if (currentCell != null && currentCell.GetExits().Count > 0)
            {
                int requiredDirection = GetRequiredGateDirection(direction);
                foreach (var gate in currentCell.GetExits())
                {
                    if (gate.GetDirection() == requiredDirection && gate.GetColorId() == block.GetColorId())
                    {
                        return currentPos; // Burada mevcut pozisyonu döndürüyoruz
                    }
                }
            }

            // Eðer mevcut hücrede gate yoksa veya uygun deðilse, normal hareket mantýðýna devam et
            Vector2Int nextPos = currentPos;

            while (true)
            {
                Vector2Int testPos = GetNextPosition(nextPos, direction);

                if (!CanMoveToPosition(testPos, block, direction))
                {
                    return nextPos;
                }

                if (HasBlockingObstacle(testPos, block))
                {
                    return nextPos;
                }

                var cell = _gridStateManager.GetCellContent(testPos.x, testPos.y);
                if (cell != null && cell.GetExits().Count > 0)
                {
                    bool hasMatchingGate = CheckGateMatch(cell, block, direction);
                    if (hasMatchingGate)
                    {
                        return testPos;
                    }
                }

                nextPos = testPos;
            }
        }


        private bool CanMoveToPosition(Vector2Int pos, Block block, SwipeDirection direction)
        {
            int length = block.GetLength();
            bool isHorizontal = block.GetOrientation() == BlockOrientation.Horizontal;

            Vector2Int checkPos;


            if (length == 1)
            {
                if (isHorizontal)
                {
                    switch (direction)
                    {
                        case SwipeDirection.Left:
                            checkPos = new Vector2Int(pos.x, pos.y);
                            break;
                        case SwipeDirection.Right:
                            checkPos = new Vector2Int(pos.x, pos.y);
                            break;
                        default:
                            return false;
                    }
                }
                else // Vertical
                {
                    switch (direction)
                    {
                        case SwipeDirection.Up:
                            checkPos = new Vector2Int(pos.x, pos.y);
                            break;
                        case SwipeDirection.Down:
                            checkPos = new Vector2Int(pos.x, pos.y);
                            break;
                        default:
                            return false;
                    }
                }
            }

            else
            {
                if (isHorizontal)
                {
                    switch (direction)
                    {
                        case SwipeDirection.Left:
                            checkPos = new Vector2Int(pos.x, pos.y);
                            break;
                        case SwipeDirection.Right:
                            checkPos = new Vector2Int(pos.x, pos.y + 1);
                            break;
                        default:
                            return false;
                    }
                }
                else // Vertical
                {
                    switch (direction)
                    {
                        case SwipeDirection.Up:
                            checkPos = new Vector2Int(pos.x, pos.y);
                            break;
                        case SwipeDirection.Down:
                            checkPos = new Vector2Int(pos.x, pos.y);
                            break;
                        default:
                            return false;
                    }
                }
            }





            if (!_gridStateManager.IsValidPosition(checkPos.x, checkPos.y))
            {
                return false;
            }

            var cell = _gridStateManager.GetCellContent(checkPos.x, checkPos.y);
            if (cell?.Block != null && cell.Block != block)
            {
                return false;
            }

            else
                return true;
        }

        private bool HasBlockingObstacle(Vector2Int pos, Block block)
        {
            int length = block.GetLength();
            bool isHorizontal = block.GetOrientation() == BlockOrientation.Horizontal;

            for (int i = 0; i < length; i++)
            {
                Vector2Int checkPos = isHorizontal
                    ? new Vector2Int(pos.x, pos.y + i)
                    : new Vector2Int(pos.x + i, pos.y);

                var cell = _gridStateManager.GetCellContent(checkPos.x, checkPos.y);
                if (cell?.Block != null && cell.Block != block)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckGateMatch(CellContent cell, Block block, SwipeDirection direction)
        {
            if (cell == null) return false;

            int requiredDirection = GetRequiredGateDirection(direction);
            foreach (var gate in cell.GetExits())
            {
                if (gate.GetDirection() == requiredDirection && gate.GetColorId() == block.GetColorId())
                {
                    return true;
                }
            }
            return false;
        }

        private void MoveBlock(Block block, Vector2Int targetPos, SwipeDirection direction)
        {
            if (!_moveLimitManager.TryUseMove())
            {
                return; // Hareket limiti bittiyse hareketi iptal et
            }

            Vector2Int startPos = block.GetGridPosition();
            Vector3 currentWorldPos = block.GetTransform().position;
            Vector3 moveOffset;

            // Length 2 olan bloklar için özel hesaplama
            if (block.GetLength() == 2 && block.GetOrientation() == BlockOrientation.Vertical)
            {
                Vector2Int visualStartPos = startPos;
                Vector2Int visualTargetPos = targetPos;

                // Sadece aþaðý hareket ederken görsel pozisyonu yukarý al
                if (direction == SwipeDirection.Down)
                {
                    visualTargetPos = new Vector2Int(targetPos.x - 1, targetPos.y);
                }

                moveOffset = CalculateMoveOffset(visualStartPos, visualTargetPos);
            }
            else
            {
                moveOffset = CalculateMoveOffset(startPos, targetPos);
            }

            Vector3 targetWorldPos = currentWorldPos + moveOffset;

            var targetCell = _gridStateManager.GetCellContent(targetPos.x, targetPos.y);
            bool willDisappear = CheckGateMatch(targetCell, block, direction);

            block.GetTransform()
                .DOMove(targetWorldPos, _moveDuration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    if (willDisappear)
                    {
                        HandleBlockDisappear(block, startPos, targetPos);
                    }
                    else
                    {
                        _gridStateManager.UpdateBlockPosition(block, startPos, targetPos);
                        block.Setup(targetPos, block.GetAllowedDirections());
                    }
                    _gridStateManager.DebugPrintGrid();
                });
        }

        private void HandleBlockDisappear(Block block, Vector2Int startPos, Vector2Int targetPos)
        {

            // Önce grid state'i güncelle
            _gridStateManager.ClearBlockFromGrid(startPos, block.GetOrientation(), block.GetLength());

            // Yok olma animasyonu
            block.GetTransform()
                .DOScale(Vector3.zero, _disappearDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    block.Inactive();
                    _gridStateManager.DebugPrintGrid();

            // Blok kaybolduktan sonra kalan bloklarý kontrol et
            if (!_gridStateManager.CheckRemainingBlocks())
                    {
                        _gameManager.Win();
                    }
                });
        }
        private Vector2Int GetNextPosition(Vector2Int currentPos, SwipeDirection direction)
        {
            return direction switch
            {
                SwipeDirection.Up => new Vector2Int(currentPos.x - 1, currentPos.y),
                SwipeDirection.Down => new Vector2Int(currentPos.x + 1, currentPos.y),
                SwipeDirection.Left => new Vector2Int(currentPos.x, currentPos.y - 1),
                SwipeDirection.Right => new Vector2Int(currentPos.x, currentPos.y + 1),
                _ => currentPos
            };
        }

        private Vector3 CalculateMoveOffset(Vector2Int startPos, Vector2Int targetPos)
        {
            Vector2Int delta = targetPos - startPos;
            return new Vector3(
                delta.y * _cellSize,  // X ekseni (col)
                0f,                   // Y ekseni
                -delta.x * _cellSize  // Z ekseni (row)
            );
        }

        private int GetRequiredGateDirection(SwipeDirection direction)
        {
            return direction switch
            {
                SwipeDirection.Up => 0,    // Yukarý hareket -> Yukarý bakan gate
                SwipeDirection.Right => 1, // Saða hareket -> Saða bakan gate
                SwipeDirection.Down => 2,  // Aþaðý hareket -> Aþaðý bakan gate
                SwipeDirection.Left => 3,  // Sola hareket -> Sola bakan gate
                _ => -1
            };
        }

        private void ResetSwipe()
        {
            _isSwiping = false;
            _selectedBlock = null;
        }

        private void OnDestroy()
        {
            if (_serviceManager != null)
            {
                var touchManager = _serviceManager.GetCoreManager<TouchManager>();
                if (touchManager != null)
                {
                    touchManager.Down -= Down;
                    touchManager.Up -= Up;
                }
            }
        }

        private enum SwipeDirection
        {
            None,
            Up,
            Down,
            Left,
            Right
        }
    }
}