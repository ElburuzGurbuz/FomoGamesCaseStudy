using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace InGame
{
    public class InGameMoneyManager : InGameService
    {
        private MoneyTextUIItem _moneyTextInCorePanel, _moneyTextInWinPanel;

        public override IEnumerator Initialize()
        {
            var uiManager = _serviceManager.GetCoreManager<UIManager>();
            _moneyTextInCorePanel = uiManager.FindItem<CoreInGamePanel, MoneyTextUIItem>();
            _moneyTextInWinPanel = uiManager.FindItem<WinPanel, MoneyTextUIItem>();
            UpdateText();

            yield return _waitForEndFrame;
        }

        public void UpdateText()
        {
            var moneyState = _gameManager.GetSettings().UISettings.WinLevelWithMoney;
            if (moneyState)
            {
                var money = _dataManager.GetValue<int>("Money");
                _moneyTextInCorePanel.SetText(money);
                _moneyTextInWinPanel.SetText(money);
            }
        }
    }
}
