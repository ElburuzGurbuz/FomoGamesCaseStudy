using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class LCRegularRandomness
    {
        private static GeneralGameSettings _gameSettings;
        private static List<int> _levelOrder;

        public static void Init()
        {
            _levelOrder = new List<int>();
            _gameSettings = ServiceManager.Instance.GetCoreManager<GameManager>().GetSettings();
            SetLevelOrderList();
        }

        public static int Handle(int levelData)
        {
            return _levelOrder[levelData  % _gameSettings.MaxLevel];
        }

        private static void SetLevelOrderList()
        {
            for (int i = 1; i < _gameSettings.MaxLevel+1; i++)
            {
                _levelOrder.Add(i);
            }
            _levelOrder.Shuffle();
        }
    }

}
