using System.Collections;
using UnityEngine;
using InGame;
using Core;
using ColorBlockData;
using System.IO;

namespace ColorBlocks
{
    public class ColorBlockInfoManager : InGameService
    {
        private const string LEVELS_FOLDER = "Assets/Levels/";
        private LevelData _currentLevelData;

        public override IEnumerator Initialize()
        {
            LoadCurrentLevel();
            yield return _waitForEndFrame;
        }

        private void LoadCurrentLevel()
        {
            var levelNumber = _dataManager.GetLevelName();
            var levelPath = Path.Combine(LEVELS_FOLDER, $"Level{levelNumber}.json");


            if (!File.Exists(levelPath))
            {
                return;
            }

            try
            {
                string jsonContent = File.ReadAllText(levelPath);
                _currentLevelData = JsonUtility.FromJson<LevelData>(jsonContent);
            }
            catch (System.Exception e)
            {

            }
        }

        public LevelData GetCurrentLevelData()
        {
            return _currentLevelData;
        }
    }
}