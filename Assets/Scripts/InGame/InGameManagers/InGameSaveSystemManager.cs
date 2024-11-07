using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System;

namespace InGame
{
    public class InGameSaveSystemManager : InGameService
    {
        private Data _inGameData;

        [SerializeField]
        private List<Save> _saveList;
        private Dictionary<string, Save> _saveDictionary;

        private Dictionary<string, Data<float>> _floatDictionary;
        private Dictionary<string, Data<int>> _intDictionary;
        private Dictionary<string, Data<bool>> _boolDictionary;
        private Dictionary<string, Data<string>> _stringDictionary;
        private Dictionary<string, Data<Vector3>> _vector3Dictionary;
        private Dictionary<string, Data<Vector2>> _vector2Dictionary;

        public override IEnumerator Initialize()
        {
            yield return _waitForEndFrame;

            if (HasInGameListState())
                yield break;

            CreateSaveDictionary();

            _inGameData = new Data();

            if (HasInGameSaveSystem(out string inGameSave))
            {
                Assign(inGameSave);
            }
            else
            {
                Register();
            }
            CreateInGameDataDictionary();
        }

        private void CreateInGameDataDictionary()
        {
            _floatDictionary = new Dictionary<string, Data<float>>();
            _intDictionary = new Dictionary<string, Data<int>>();
            _boolDictionary = new Dictionary<string, Data<bool>>();
            _stringDictionary = new Dictionary<string, Data<string>>();
            _vector3Dictionary = new Dictionary<string, Data<Vector3>>();
            _vector2Dictionary = new Dictionary<string, Data<Vector2>>();

            foreach (var item in _inGameData.FD)
                _floatDictionary.Add(item.K, item);

            foreach (var item in _inGameData.ID)
                _intDictionary.Add(item.K, item);

            foreach (var item in _inGameData.BD)
                _boolDictionary.Add(item.K, item);

            foreach (var item in _inGameData.SD)
                _stringDictionary.Add(item.K, item);

            foreach (var item in _inGameData.V3D)
                _vector3Dictionary.Add(item.K, item);

            foreach (var item in _inGameData.V2D)
                _vector2Dictionary.Add(item.K, item);
        }

        private void Assign(string inGameSave)
        {
            JsonUtility.FromJsonOverwrite(inGameSave, _inGameData);

            foreach (var item in _inGameData.BD)
                _saveDictionary[item.K].SetData(item.V);

            foreach (var item in _inGameData.ID)
                _saveDictionary[item.K].SetData(item.V);

            foreach (var item in _inGameData.FD)
                _saveDictionary[item.K].SetData(item.V);

            foreach (var item in _inGameData.SD)
                _saveDictionary[item.K].SetData(item.V);

            foreach (var item in _inGameData.V3D)
                _saveDictionary[item.K].SetData(item.V);

            foreach (var item in _inGameData.V2D)
                _saveDictionary[item.K].SetData(item.V);
        }

        private void CreateSaveDictionary()
        {
            _saveDictionary = new Dictionary<string, Save>();

            foreach (var item in _saveList)
                CreateSaveDictionary(item);

        }

        private void CreateSaveDictionary(Save item)
        {
            if (item.BooleanData.K != string.Empty)
                _saveDictionary.Add(item.BooleanData.K, item);

            if (item.FloatData.K != string.Empty)
                _saveDictionary.Add(item.FloatData.K, item);

            if (item.IntegerData.K != string.Empty)
                _saveDictionary.Add(item.IntegerData.K, item);

            if (item.StringData.K != string.Empty)
                _saveDictionary.Add(item.StringData.K, item);

            if (item.Vector3Data.K != string.Empty)
                _saveDictionary.Add(item.Vector3Data.K, item);

            if (item.Vector2Data.K != string.Empty)
                _saveDictionary.Add(item.Vector2Data.K, item);
        }

        private void Register()
        {
            foreach (var item in _saveList)
                Register(item);
        }

        private void Register(Save item)
        {
            _inGameData.ID = new List<Data<int>>();
            _inGameData.FD = new List<Data<float>>();
            _inGameData.SD = new List<Data<string>>();
            _inGameData.BD = new List<Data<bool>>();
            _inGameData.V3D = new List<Data<Vector3>>();
            _inGameData.V2D = new List<Data<Vector2>>();

            if (item.IntegerData.K != string.Empty)
            {
                _inGameData.ID.Add(new Data<int>()
                {
                    K = item.IntegerData.K,
                    V = item.IntegerData.V
                });
                item.SetData(item.IntegerData.V);
            }
            if (item.FloatData.K != string.Empty)
            {
                _inGameData.FD.Add(new Data<float>()
                {
                    K = item.FloatData.K,
                    V = item.FloatData.V
                });
                item.SetData(item.FloatData.V);
            }
            if (item.StringData.K != string.Empty)
            {
                _inGameData.SD.Add(new Data<string>()
                {
                    K = item.StringData.K,
                    V = item.StringData.V
                });
                item.SetData(item.StringData.V);
            }
            if (item.BooleanData.K != string.Empty)
            {
                _inGameData.BD.Add(new Data<bool>()
                {
                    K = item.BooleanData.K,
                    V = item.BooleanData.V
                });
                item.SetData(item.BooleanData.V);
            }
            if (item.Vector3Data.K != string.Empty)
            {
                _inGameData.V3D.Add(new Data<Vector3>()
                {
                    K = item.Vector3Data.K,
                    V = item.Vector3Data.V
                });
                item.SetData(item.Vector3Data.V);
            }
            if (item.Vector2Data.K != string.Empty)
            {
                _inGameData.V2D.Add(new Data<Vector2>()
                {
                    K = item.Vector2Data.K,
                    V = item.Vector2Data.V
                });
                item.SetData(item.Vector2Data.V);
            }
        }

        private bool HasInGameSaveSystem(out string inGameSave)
        {
            inGameSave = _dataManager.GetValue<string>(_dataManager.GetInGameSaveKey);
            return inGameSave != string.Empty;
        }

        private bool HasInGameListState()
        {
            return _saveList == null || _saveList.Count == 0;
        }

        public void SetData<T>(string key, T value)
        {
            if (typeof(T) == typeof(bool))
            {
                var b = (bool)(object)value;
                _boolDictionary[key].V = b;
                _saveDictionary[key].SetData(b);
            }

            else if (typeof(T) == typeof(int))
            {
                var i = (int)(object)value;
                _intDictionary[key].V = i;
                _saveDictionary[key].SetData(i);
            }

            else if (typeof(T) == typeof(float))
            {
                var f = (float)(object)value;
                _floatDictionary[key].V = f;
                _saveDictionary[key].SetData(f);
            }

            else if (typeof(T) == typeof(string))
            {
                var s = (string)(object)value;
                _stringDictionary[key].V = s;
                _saveDictionary[key].SetData(s);
            }

            else if (typeof(T) == typeof(Vector3))
            {
                var v3 = (Vector3)(object)value;
                _vector3Dictionary[key].V = v3;
                _saveDictionary[key].SetData(v3);
            }

            else if (typeof(T) == typeof(Vector2))
            {
                var v2 = (Vector2)(object)value;
                _vector2Dictionary[key].V = v2;
                _saveDictionary[key].SetData(v2);
            }

            var str = JsonUtility.ToJson(_inGameData);
            _dataManager.SetValue(_dataManager.GetInGameSaveKey, str);
        }
        public T GetData<T>(string key)
        {
            if (typeof(T) == typeof(bool))
            {
                return (T)(object)_boolDictionary[key].V;
            }

            if (typeof(T) == typeof(int))
            {
                return (T)(object)_intDictionary[key].V;
            }

            if (typeof(T) == typeof(float))
            {
                return (T)(object)_floatDictionary[key].V;
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)_stringDictionary[key].V;
            }

            if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)_vector3Dictionary[key].V;
            }

            if (typeof(T) == typeof(Vector2))
            {
                return (T)(object)_vector2Dictionary[key].V;
            }

            return (T)(object)string.Empty;
        }

    }
}

