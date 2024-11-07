using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

namespace InGame
{
    public class InGameCameraManagers : InGameService
    {
        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        public override IEnumerator Initialize()
        {
            yield return _waitForEndFrame;
        }
        public void SetCameraSettings(Vector3 pos,Vector3 rot,float fov)
        {
            var t = _virtualCamera.transform;
            t.DOMove(pos,0.5f);
            t.DORotateQuaternion(Quaternion.Euler(rot),0.5f);
            DOTween.To(() => _virtualCamera.m_Lens.FieldOfView, x => _virtualCamera.m_Lens.FieldOfView = x, fov, 0.5f);
        }
    }

}
