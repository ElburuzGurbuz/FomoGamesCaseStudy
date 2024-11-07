using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core
{
    public class WinPanel : UIPanel
    {
        [SerializeField]
        private List<UIItem> _moneyElements;

        [SerializeField]
        private List<UIItem> _elementsWithMoney;

        [SerializeField]
        private GameObject _winWithMoneyText;

        [SerializeField]
        private GameObject _winWithoutMoneyText;

        [SerializeField]
        private TextMeshProUGUI _earnMoneyText;

        [SerializeField]
        private Transform _moneyTarget;

        [SerializeField]
        private Transform _createTarget;

        private ServiceManager _serviceManager;
        private GameManager _gameManager;
        private DataManager _dataManager;

        private MoneyPool _moneyPool;
        private WaitForSeconds _waitForSecond;

        private int _money = 0;
        private bool _waitForMoney = true;

        public override void InitializePanels()
        {
            base.InitializePanels();

            _serviceManager = ServiceManager.Instance;

            _gameManager = _serviceManager.GetCoreManager<GameManager>();
            _dataManager = _serviceManager.GetCoreManager<DataManager>();

            var showMoney = _gameManager.GetSettings().UISettings.ShowMoney;

            if (showMoney)
                OpenMoneyElements();
            else
                CloseMoneyElements();

            _moneyPool = _serviceManager.GetCoreManager<PoolManager>().
                GetPool<MoneyPool>();

            _waitForSecond = new WaitForSeconds(2);
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

        public void WinLevelWithMoney()
        {
            foreach (var item in _elementsWithMoney)
            {
                item.OpenItem();

                _winWithMoneyText.SetActive(true);
                _winWithoutMoneyText.SetActive(false);
            }
        }

        public void WinLevelWithoutMoney()
        {
            foreach (var item in _elementsWithMoney)
            {
                item.CloseItem();

                _winWithMoneyText.SetActive(false);
                _winWithoutMoneyText.SetActive(true);
            }
        }

        public void OnClickForWinButton()
        {
            if (!_waitForMoney)
                return;

            _waitForMoney = false;

            var check = _gameManager.GetSettings().UISettings.WinLevelWithMoney;
            if (check)
            {
                StartCoroutine(WinMoney());
            }
            else
            {
                _gameManager.Win(false);
                _waitForMoney = true;
            }
        }
        private IEnumerator WinMoney()
        {
            Tweener finishMoneyMovement = null;
            int counterForMoney = _money;
            counterForMoney = Mathf.Clamp(counterForMoney, 0, 25);

            for (int i = 0; i < counterForMoney; i++)
            {
                var money = _moneyPool.GetFromPool();
                finishMoneyMovement = money.Move(_moneyTarget.position,
                    _createTarget.position, _moneyTarget.localScale, i);
            }

            if (finishMoneyMovement != null)
                yield return finishMoneyMovement.WaitForCompletion();

            UpdateMoney();

            yield return _waitForSecond;

            _waitForMoney = true;
            _gameManager.Win(false);
        }

        public void SetMoney(int money)
        {
            _earnMoneyText.text = money.ToString();
            _money = money;
        }
        public void UpdateMoney()
        {
            var newMoney = _dataManager.GetValue<int>("Money") + _money;
            _dataManager.SetValue("Money", newMoney);
            _gameManager.GetInGameManager<InGame.InGameMoneyManager>().UpdateText();
        }
    }
}
