using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGame;

namespace Core
{
    public class CoreInGamePanel : UIPanel
    {
        [SerializeField]
        private List<UIItem> _moneyElements;

        public override void InitializePanels()
        {
            base.InitializePanels();

            var showMoney=ServiceManager.Instance.GetCoreManager<GameManager>().
                GetSettings().UISettings.ShowMoney;

            if (showMoney)
                OpenMoneyElements();
            else
                CloseMoneyElements();
        }

        public void CloseMoneyElements()
        {
            foreach (var item in _moneyElements)
            {
                item.CloseItem();
            }
        }
        public void OpenMoneyElements()
        {
            foreach (var item in _moneyElements)
            {
                item.OpenItem();
            }
        }
    }
}
