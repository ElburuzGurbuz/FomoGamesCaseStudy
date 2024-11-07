using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "UISettings", menuName = "FomoGames/UI Settings", order = 1)]
    public class UISettings : ScriptableObject
    {
        [OnValueChanged("MoneyChangeCallback")]
        public bool ShowMoney = true;

        [OnValueChanged("LevelIndicatorChangeCallback")]
        public bool ShowLevelIndicator = true;

        [OnValueChanged("ShowSettingsIndicatorCallback")]
        public bool ShowSettingsIndicator = true;

        [OnValueChanged("WinLevelWithMoneyCallback")]
        public bool WinLevelWithMoney = true;

        public string LevelNameLevelIndicator = "LVL";

        [OnValueChanged("ShowSettingElementListCallback")]
        public List<UISettingHelper> ShowSettingElementList;

        public void MoneyChangeCallback()
        {
            var coreInGamePanel = FindObjectOfType<CoreInGamePanel>();
            if (ShowMoney)
                coreInGamePanel.OpenMoneyElements();
            else
                coreInGamePanel.CloseMoneyElements();

            var winPanel = FindObjectOfType<WinPanel>();
            if (ShowMoney)
                winPanel.OpenMoneyElements();
            else
                winPanel.CloseMoneyElements();
        }
        public void LevelIndicatorChangeCallback()
        {
            var levelIndicatorTextUIItem = FindObjectOfType<LevelIndicatorTextUIItem>();

            if (ShowLevelIndicator)
                levelIndicatorTextUIItem.OpenText();
            else
                levelIndicatorTextUIItem.CloseText();
        }
        public void ShowSettingElementListCallback()
        {
            var _settingsMenuUIItem = FindObjectOfType<SettingsMenuUIItem>();
            _settingsMenuUIItem.CloseSettingElements();
            _settingsMenuUIItem.OpenSettingElements(this);
        }
        public void ShowSettingsIndicatorCallback()
        {
            var setting = FindObjectOfType<SettingsMenuUIItem>();

            if (ShowSettingsIndicator)
            {
                setting.OpenItem();
                setting.OpenSettingElements(this);
            }
            else
            {
                setting.CloseItem();
                setting.CloseSettingElements();
            }

        }
        public void WinLevelWithMoneyCallback()
        {
            var winPanel = FindObjectOfType<WinPanel>();
            if (WinLevelWithMoney)
                winPanel.WinLevelWithMoney();
            else
                winPanel.WinLevelWithoutMoney();
        }
    }
    [System.Serializable]
    public class UISettingHelper
    {
        public UISettingsEnum SettingType;
        public bool ShowSettingElement = true;
    }

    public enum UISettingsEnum
    {
        Haptic,
        Sound,
        Restart
    }

}
