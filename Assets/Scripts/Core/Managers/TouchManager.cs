using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public class TouchManager : Service
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private TMController _tmController;

        [SerializeField]
        private List<TMController> _tmControllersList;

        public event Action<TouchData> Down, Set, Up;
        public override IEnumerator Initialize()
        {
            if (_camera == null)
            {
                _camera = Camera.main;

                Debug.LogError("Camera has not been found. " +
                    "Now I find this instead of you but this is not good method.");
            }
            foreach (var item in _tmControllersList)
            {
                if (item == _tmController)
                    continue;

                item.gameObject.SetActive(false);
            }

            _tmController.Initialize();

            yield return _waitForEndOfFrame;
        }
        public Camera GetCamera()
        {
            return _camera;
        }
        public bool IsPointerOverUI()
        {
            bool b;

#if UNITY_EDITOR
            b = IsPointerOverUIForMouse();

#else
            b = IsPointerOverUIForTouch();
#endif
            return b;
        }
        private bool IsPointerOverUIForMouse()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        private bool IsPointerOverUIForTouch()
        {
            bool result = false;
            for (int i = 0; i < Input.touchCount; i++)
                result |= EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId);
            return result;
        }
        public void DownCallback(TouchData data) => Down?.Invoke(data);
        public void SetCallback(TouchData data) => Set?.Invoke(data);
        public void UpCallback(TouchData data) => Up?.Invoke(data);
    }
    public class TouchData
    {
        public Vector3 ClickData;
        public RaycastHit Hit;
        public bool HitCheck;
        public Vector3 HitStartPos;

        public float Horizontal, Vertical;
    }
}

