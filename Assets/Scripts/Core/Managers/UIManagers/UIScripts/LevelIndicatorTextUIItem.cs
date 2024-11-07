using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core
{
    public class LevelIndicatorTextUIItem : UIItem
    {
        [SerializeField]
        private TextMeshProUGUI _moneyText;

        private GameManager _gameManager;
        private UISettings _uiSettings;

        public override void InitializeUIItems()
        {
            base.InitializeUIItems();

            _gameManager = ServiceManager.Instance.GetCoreManager<GameManager>();
            _uiSettings = _gameManager.GetSettings().UISettings;

            var textIndicator = _uiSettings.ShowLevelIndicator;

            if (textIndicator)
                OpenText();
            else
                CloseText();
        }

        public void SetText(int level)
        {
            _moneyText.text = $"{_uiSettings.LevelNameLevelIndicator} {level}";
        }
        public void OpenText()
        {
            OpenItem();
        }
        public void CloseText()
        {
            CloseItem();
        }
    }
}