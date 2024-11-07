using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace InGame
{
    public class InGameReadyToPlayManager : InGameService
    {
        private UIManager _uiManager;
        public override IEnumerator Initialize()
        {
            _uiManager = _serviceManager.GetCoreManager<UIManager>();

            _uiManager.OpenPanel<InGamePanel>();
            _uiManager.OpenPanel<CoreInGamePanel>();

            yield return _waitForEndFrame;
        }
    }

}
