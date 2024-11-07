using UnityEngine;
using Core;

namespace InGame
{
    public class ExitBlock : MonoBehaviour, IPoolable
    {
        [Header("Exit Properties")]
        [SerializeField] private int _colorId;
        [SerializeField] private int _direction;  // 0: Up, 1: Right, 2: Down, 3: Left

        private Vector2Int _gridPosition;

        public void Active()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }

        public void Create() { }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void Inactive()
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        public void Setup(Vector2Int gridPosition)
        {
            _gridPosition = gridPosition;
        }

        // Getters
        public int GetColorId() => _colorId;
        public int GetDirection() => _direction;
        public Vector2Int GetGridPosition() => _gridPosition;
    }
}