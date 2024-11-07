using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface IPoolable
    {
        public void Create();
        public void Active();
        public void Inactive();

        public GameObject GetGameObject();
        public Transform GetTransform();
    }
}
