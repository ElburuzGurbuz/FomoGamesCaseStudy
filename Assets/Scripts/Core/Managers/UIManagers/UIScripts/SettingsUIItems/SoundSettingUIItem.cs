using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class SoundSettingUIItem : UIItem
    {
        private DataManager _dataManager;
        private SoundManager _soundManager;

        [SerializeField]
        private Sprite _enableSprite, _disableSprite;

        [SerializeField]
        private Image _image;

        public override void InitializeUIItems()
        {
            base.InitializeUIItems();

            var service = ServiceManager.Instance;

            _soundManager = service.GetCoreManager<SoundManager>();
            _dataManager = service.GetCoreManager<DataManager>();

            var isEnable = _dataManager.GetValue<bool>(DataManager.SoundKey);
            SetSoundSetting(isEnable);
        }
        private void SetSoundSetting(bool isEnable)
        {
            _image.sprite = isEnable ? _enableSprite : _disableSprite;
            _soundManager.SetActivation(isEnable);
        }

        public void SoundOnClick()
        {
            var newState = !_dataManager.GetValue<bool>(DataManager.SoundKey);
            _dataManager.SetValue(DataManager.SoundKey, newState);
            SetSoundSetting(newState);
        }
    }
}
