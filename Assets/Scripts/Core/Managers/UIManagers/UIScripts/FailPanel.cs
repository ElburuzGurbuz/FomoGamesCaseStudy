using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class FailPanel : UIPanel
    {

        private ServiceManager _serviceManager;
        private GameManager _gameManager;
        public override void InitializePanels()
        {
            base.InitializePanels();

            _serviceManager = ServiceManager.Instance;
            _gameManager = _serviceManager.GetCoreManager<GameManager>();
        }

        public void OnClickForFail()
        {
            _gameManager.Fail(false);
        }
    }
}
