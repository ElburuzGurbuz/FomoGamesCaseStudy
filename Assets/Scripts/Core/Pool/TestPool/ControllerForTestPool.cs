using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ControllerForTestPool : MonoBehaviour, IPoolable
    {
        public void Active()
        {
           
        }

        public void Create()
        {
            
        }

        public void Inactive()
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
        public void Movement() { }
    }
}
