using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class LCFullRandomness
    {
        private static GeneralGameSettings _gameSettings;
        private static int _previousLevelIndex;

        public static void Init()
        {
            _gameSettings = ServiceManager.Instance.GetCoreManager<GameManager>().GetSettings();
            _previousLevelIndex = _gameSettings.MaxLevel;
        }

        public static int Handle(int levelData)
        {
            var levelIndex = CreateRandomLevelIndex();
            _previousLevelIndex = levelIndex;
            return levelIndex;
        }

        private static int CreateRandomLevelIndex()
        {
            int index;
            do
            {
                index = Random.Range(1, _gameSettings.MaxLevel+1);
            } while (index == _previousLevelIndex);

            return index;
        }
    }

}
