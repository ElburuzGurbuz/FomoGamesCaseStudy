using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class SettingsMenuUIItem : UIItem
    {
        [SerializeField]
        private List<UISettingElementHolder> _settingElements;

        [SerializeField]
        private List<Transform> _settingUIItemsPositions;

        private UISettings _uiSettings;

        private bool _enable = false;

        [System.Serializable]
        public class UISettingElementHolder
        {
            public UISettingsEnum SettingType;
            public GameObject Item;
        }

        public override void InitializeUIItems()
        {
            base.InitializeUIItems();

            _uiSettings = ServiceManager.Instance.GetCoreManager<GameManager>().GetSettings().UISettings;

            if (_uiSettings.ShowSettingsIndicator)
                OpenItem();
            else
                CloseItem();

            CloseSettingElements();
        }

        public void SettingButtonOnClick()
        {
            _enable = !_enable;
            if (_enable)
                OpenSettingElements(_uiSettings);
            else
                CloseSettingElements();
        }

        public void OpenSettingElements(UISettings uiSettings)
        {
            int positionIndex = 0;
            foreach (var item in _settingElements)
            {
                var type = item.SettingType;
                var uiItem = uiSettings.ShowSettingElementList.Find(x => x.SettingType == type);
                if (uiItem.ShowSettingElement)
                {
                    var i = item.Item;
                    i.transform.position = _settingUIItemsPositions[positionIndex].position;
                    i.SetActive(true);
                    positionIndex++;
                }
            }
        }
        public void CloseSettingElements()
        {
            foreach (var item in _settingElements)
            {
                item.Item.SetActive(false);
            }
        }
    }
}
