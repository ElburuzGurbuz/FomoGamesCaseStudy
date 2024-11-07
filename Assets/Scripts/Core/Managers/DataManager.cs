using System.Collections;
using System.Collections.Generic;
//using Newtonsoft.Json;
using UnityEngine;

namespace Core
{
    public partial class DataManager : Service
    {
        private const string SaveKey = "Save";
        private const string LevelKey = "Level";
        private const string LevelName = "LevelName";
        public const string WholeGameCompleted = "WholeGameCompleted";
        private const string InGameSaveSystemKey = "InGameSave";
        public const string HapticKey = "Haptic";
        public const string SoundKey = "Sound";
        public const string LevelStartEventKey = "lvl";
        public const string Money = "Money";

        private Data _data;
        private GameManager _gameManager;

        private GeneralGameSettings _generalGameSettings;

        public string GetInGameSaveKey
        {
            get
            {
                return InGameSaveSystemKey + GetLevelName();
            }
        }

        public override IEnumerator Initialize()
        {
            yield return _waitForEndOfFrame;

            _gameManager = _serviceManager.GetCoreManager<GameManager>();
            _generalGameSettings = _gameManager.GetSettings();

            _data = new Data();

            var save = GetSave();
            if (save != string.Empty)
                FromJsonOverwrite(save);
            else
                InitSaveSystem();

            ArrangementsForData();

        }
        private void InitSaveSystem()
        {
            _data.BD = new List<Data<bool>>();
            _data.ID = new List<Data<int>>();
            _data.FD = new List<Data<float>>();
            _data.SD = new List<Data<string>>();
            _data.V3D = new List<Data<Vector3>>();
            _data.V2D = new List<Data<Vector2>>();
        }

        public void ArrangementsForData()
        {
            var listString = ArrangeList(_generalGameSettings.StringData, _data.SD);
            var listInteger = ArrangeList(_generalGameSettings.IntegerData, _data.ID);
            var listFloat = ArrangeList(_generalGameSettings.FloatData, _data.FD);
            var listBoolean = ArrangeList(_generalGameSettings.BooleanData, _data.BD);
            var listVector3 = ArrangeList(_generalGameSettings.Vector3Data, _data.V3D);
            var listVector2 = ArrangeList(_generalGameSettings.Vector2Data, _data.V2D);

            _data.SD.AddRange(listString);
            _data.ID.AddRange(listInteger);
            _data.FD.AddRange(listFloat);
            _data.BD.AddRange(listBoolean);
            _data.V3D.AddRange(listVector3);
            _data.V2D.AddRange(listVector2);

            AddData(LevelKey, 1);
            AddData(LevelName, 1);
            AddData(WholeGameCompleted, false);
            AddData(Money, 0);
            AddData(HapticKey, true);
            AddData(SoundKey, true);

            for (int i = 1; i <= _generalGameSettings.MaxLevel; i++)
            {
                AddData(InGameSaveSystemKey+i, string.Empty);
            }

            AddDataForLevelStartEvent();
        }
        public void AddDataForLevelStartEvent()
        {
            var maxLevel = _generalGameSettings.MaxLevel;
            for (int i = 1; i <= maxLevel; i++)
            {
                var newKey = $"{LevelStartEventKey}{i}";
                AddData(newKey, false);
            }
        }
        private List<Data<T>> ArrangeList<T>(List<Data<T>> list, List<Data<T>> mainData)
        {
            var _list = new List<Data<T>>();

            foreach (var item in list)
                if (mainData.Find(x => x.K == item.K) == null)
                {
                    Data<T> newItem = new Data<T>();
                    newItem.K = item.K;
                    newItem.V = item.V;
                    _list.Add(newItem);
                }
            return _list;
        }
        private void AddData<T>(string key, T value)
        {
            if (typeof(T) == typeof(bool))
                AddBoolData(key, (bool)(object)value);

            if (typeof(T) == typeof(int))
                AddIntData(key, (int)(object)value);

            if (typeof(T) == typeof(float))
                AddFloatData(key, (float)(object)value);

            if (typeof(T) == typeof(string))
                AddStringData(key, (string)(object)value);

            if (typeof(T) == typeof(Vector3))
                AddVector3Data(key, (Vector3)(object)value);

            if (typeof(T) == typeof(Vector2))
                AddVector2Data(key, (Vector2)(object)value);
        }

        #region AddDataForTypes
        private void AddBoolData(string newKey, bool b)
        {
            if (CheckBooleanData(newKey) == null)
                _data.BD.Add(new Data<bool>() { K = newKey, V = b });
        }

        private void AddIntData(string newKey, int i)
        {
            if (CheckIntData(newKey) == null)
                _data.ID.Add(new Data<int>() { K = newKey, V = i });
        }

        private void AddFloatData(string newKey, float f)
        {
            if (CheckFloatData(newKey) == null)
                _data.FD.Add(new Data<float>() { K = newKey, V = f });
        }

        private void AddStringData(string newKey, string s)
        {
            if (CheckStringData(newKey) == null)
                _data.SD.Add(new Data<string>() { K = newKey, V = s });
        }
        private void AddVector3Data(string newKey, Vector3 s)
        {
            if (CheckVector3Data(newKey) == null)
                _data.V3D.Add(new Data<Vector3>() { K = newKey, V = s });
        }
        private void AddVector2Data(string newKey, Vector2 s)
        {
            if (CheckVector2Data(newKey) == null)
                _data.V2D.Add(new Data<Vector2>() { K = newKey, V = s });
        }
        #endregion
        #region CheckData
        private Data<bool> CheckBooleanData(string s)
        {
            return _data.BD.Find(x => x.K == s);
        }

        private Data<int> CheckIntData(string s)
        {
            return _data.ID.Find(x => x.K == s);
        }

        private Data<float> CheckFloatData(string s)
        {
            return _data.FD.Find(x => x.K == s);
        }

        private Data<string> CheckStringData(string s)
        {
            return _data.SD.Find(x => x.K == s);
        }

        private Data<Vector3> CheckVector3Data(string s)
        {
            return _data.V3D.Find(x => x.K == s);
        }

        private Data<Vector2> CheckVector2Data(string s)
        {
            return _data.V2D.Find(x => x.K == s);
        }
        #endregion
        private void FromJsonOverwrite(string save)
        {
            JsonUtility.FromJsonOverwrite(save, _data);
        }

        private string ToJson()
        {
            return JsonUtility.ToJson(_data);
        }
        public void LevelStartEventClear()
        {
            var maxLevel = _generalGameSettings.MaxLevel;
            for (int i = 1; i <= maxLevel; i++)
            {
                var newKey = $"{LevelStartEventKey}{i}";
                SetValue(newKey, false);
            }
        }
    }

    //Setter methods implementations
    public partial class DataManager
    {
        public void SetLevel(int value)
        {
            SetValue(LevelKey, value);
        }
        public void SetWholeGameCompleted(bool value)
        {
            SetValue(WholeGameCompleted, value);
        }
        public void SetLevelName(int value)
        {
            SetValue(LevelName, value);
        }
        public void SetValue<T>(string key, T value)
        {
            if (typeof(T) == typeof(bool))
                SourceForSetKey(_data.BD, key, (bool)(object)value);

            if (typeof(T) == typeof(int))
                SourceForSetKey(_data.ID, key, (int)(object)value);

            if (typeof(T) == typeof(float))
                SourceForSetKey(_data.FD, key, (float)(object)value);

            if (typeof(T) == typeof(string))
                SourceForSetKey(_data.SD, key, (string)(object)value);

            if (typeof(T) == typeof(Vector3))
                SourceForSetKey(_data.V3D, key, (Vector3)(object)value);

            if (typeof(T) == typeof(Vector2))
                SourceForSetKey(_data.V2D, key, (Vector2)(object)value);

            var str = ToJson();
            SetSave(str);
        }
        private void SourceForSetKey<T>(List<Data<T>> data, string key, T value)
        {
            var v = data.Find(x => x.K == key);

            if (v != null)
                v.V = value;
        }
        private void SetSave(string value)
        {
            PlayerPrefs.SetString(SaveKey, value);
        }
    }

    //Getter methods implementations
    public partial class DataManager
    {
        public int GetLevel()
        {
            return GetValue<int>(LevelKey);
        }
        public bool GetWholeGameCompleted()
        {
            return GetValue<bool>(WholeGameCompleted);
        }
        public int GetLevelName()
        {
            return GetValue<int>(LevelName);
        }
        private string GetSave()
        {
            return PlayerPrefs.GetString(SaveKey, string.Empty);
        }
        public T GetValue<T>(string key)
        {
            if (typeof(T) == typeof(bool))
                return (T)SourceForGetKey(_data.BD, key);

            if (typeof(T) == typeof(int))
                return (T)SourceForGetKey(_data.ID, key);

            if (typeof(T) == typeof(float))
                return (T)SourceForGetKey(_data.FD, key);

            if (typeof(T) == typeof(string))
                return (T)SourceForGetKey(_data.SD, key);

            if (typeof(T) == typeof(Vector3))
                return (T)SourceForGetKey(_data.V3D, key);

            if (typeof(T) == typeof(Vector2))
                return (T)SourceForGetKey(_data.V2D, key);

            return (T)(object)string.Empty;
        }
        private object SourceForGetKey<T>(List<Data<T>> data, string key)
        {
            var value = data.Find(x => x.K == key);
            if (value != null)
                return value.V;

            return string.Empty;
        }
    }
    //Ingame
    public partial class DataManager
    {
        public void ResetInGameData()
        {
            SetValue(InGameSaveSystemKey, string.Empty);
        }
        public void SetInGameData(string newValue)
        {
            SetValue(InGameSaveSystemKey, newValue);
        }
        public string GetInGameData()
        {
            return GetValue<string>(InGameSaveSystemKey);
        }
    }
}
[System.Serializable]
public class Data
{
    [NonReorderable]
    public List<Data<string>> SD;

    [NonReorderable]
    public List<Data<float>> FD;

    [NonReorderable]
    public List<Data<int>> ID;

    [NonReorderable]
    public List<Data<bool>> BD;

    [NonReorderable]
    public List<Data<Vector3>> V3D;

    [NonReorderable]
    public List<Data<Vector2>> V2D;
}

[System.Serializable]
public class Data<T>
{
    public string K;
    public T V;
}

