using System.Collections;
using UnityEngine;
using InGame;
using Core;
using ColorBlockData;

namespace InGame
{
    public class GridCell : MonoBehaviour, IPoolable
    {
        [SerializeField] private MeshRenderer _renderer;
        private Vector2Int _gridPosition;

        public void Active()
        {
            gameObject.SetActive(true);
        }

        public void Create()
        {
        }

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
            gameObject.SetActive(false);
        }

        public void SetMaterial(Material material)
        {
            if (_renderer != null)
                _renderer.material = material;
        }

        public void SetGridPosition(Vector2Int pos)
        {
            _gridPosition = pos;
        }
    }


}