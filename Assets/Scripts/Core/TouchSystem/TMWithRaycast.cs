using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class TMWithRaycast : TMController
    {
        private bool _downState = false;
        private bool _upState = false;
        private bool _setState = false;

        private TouchData _downData, _upData, _setData;

        [SerializeField]
        private TouchManager _touchManager;

        [SerializeField]
        private LayerMask _mainLayerMask;

        [SerializeField]
        private int _rayDistance = 1000;

        private Camera _camera;

        public override void Initialize()
        {
            _downData = new TouchData();
            _upData = new TouchData();
            _setData = new TouchData();
            _camera = _touchManager.GetCamera();
        }

        private void Update()
        {
            if (_touchManager.IsPointerOverUI())
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                _downState = true;
                _downData.ClickData = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                _setState = true;
                _setData.ClickData = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _upState = true;
                _upData.ClickData = Input.mousePosition;
            }
        }
        private void FixedUpdate()
        {
            if (_downState)
            {
                _downState = false;
                FillData(_downData);
                _touchManager.DownCallback(_downData);
            }
            if (_setState)
            {
                FillData(_setData);
                _touchManager.SetCallback(_setData);
            }
            if (_upState)
            {
                _upState = false;
                _setState = false;
                FillData(_upData);
                _touchManager.UpCallback(_upData);
            }
        }
        public bool RayCast(Vector3 clickPos,out RaycastHit hit,out Vector3 hitStartPos)
        {
            var ray = _camera.ScreenPointToRay(clickPos);
            hitStartPos = ray.origin;
            return Physics.Raycast(ray, out  hit, _rayDistance, _mainLayerMask);
        }
        public void FillData(TouchData data)
        {
            var c = RayCast(data.ClickData, out RaycastHit hit, out Vector3 hitStartPos);
            data.HitCheck = c;
            data.Hit = hit;
            data.HitStartPos = hitStartPos;

#if UNITY_EDITOR
            if (data.HitCheck)
                Debug.DrawLine(data.HitStartPos,data.Hit.point,Color.red);
#endif
        }
    }
}