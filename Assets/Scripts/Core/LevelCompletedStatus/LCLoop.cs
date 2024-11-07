using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class LCLoop
    {
        private static GeneralGameSettings _gameSettings;
        public static void Init()
        {
            _gameSettings = ServiceManager.Instance.GetCoreManager<GameManager>().GetSettings();
        }
        public static int Handle(int levelData)
        {
            var d = (levelData % _gameSettings.MaxLevel);
            if (d==0)
            {
                return _gameSettings.MaxLevel;
            }
            return d;
        }
    }
}

