using UnityEngine;
using Core;

namespace InGame
{
    public enum BlockOrientation
    {
        Horizontal,
        Vertical
    }

    public class Block : MonoBehaviour, IPoolable
    {
        [Header("Block Properties")]
        [SerializeField] private int _colorId;
        [SerializeField] private int _length;
        [SerializeField] private BlockOrientation _orientation;
        [SerializeField] private MeshRenderer _meshRenderer;

        private MaterialPropertyBlock _propertyBlock;
        private Vector2Int _gridPosition;
        private int[] _allowedDirections;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void Create()
        {
            if (_meshRenderer != null)
            {
                // Property block'u sýfýrla
                _propertyBlock.Clear();
                _meshRenderer.SetPropertyBlock(_propertyBlock);
            }
        }

        public void Active()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            if (_meshRenderer != null)
            {
                _meshRenderer.enabled = true;
                // Property block'u yeniden uygula
                _meshRenderer.GetPropertyBlock(_propertyBlock);
                _meshRenderer.SetPropertyBlock(_propertyBlock);
            }
        }

        public void Inactive()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }

        // Optional: Renk deðiþtirme örneði
        public void SetColor(Color color)
        {
            if (_meshRenderer != null)
            {
                _propertyBlock.SetColor("_Color", color);
                _meshRenderer.SetPropertyBlock(_propertyBlock);
            }
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void Setup(Vector2Int gridPosition, int[] allowedDirections)
        {
            _gridPosition = gridPosition;
            _allowedDirections = allowedDirections;
        }

        // Getters
        public int GetColorId() => _colorId;
        public int GetLength() => _length;
        public BlockOrientation GetOrientation() => _orientation;
        public Vector2Int GetGridPosition() => _gridPosition;
        public int[] GetAllowedDirections() => _allowedDirections;
    }


}