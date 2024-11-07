using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Core
{
    [CreateAssetMenu(fileName = "GeneralGameSettings", menuName = "FomoGames/General Game Settings", order = 1)]
    public class GeneralGameSettings : ScriptableObject
    {
        public int FrameRate = 60;
        public int MaxLevel = 20;

        public LevelCompletedStatus LevelCompletedStatus;

        [HorizontalLine(2, EColor.Orange)]
        public UISettings UISettings;

        //[HorizontalLine(2, EColor.Orange)]
        //public TouchSettings TouchSettings;

#pragma warning disable
        [SerializeField, HorizontalLine(2, EColor.Blue)]
        private bool ShowSaveList = false;

        [ShowIf("ShowSaveList")]
        public List<Data<string>> StringData;

        [ShowIf("ShowSaveList")]
        public List<Data<int>> IntegerData;

        [ShowIf("ShowSaveList")]
        public List<Data<float>> FloatData;

        [ShowIf("ShowSaveList")]
        public List<Data<bool>> BooleanData;

        [ShowIf("ShowSaveList")]
        public List<Data<Vector3>> Vector3Data;

        [ShowIf("ShowSaveList")]
        public List<Data<Vector2>> Vector2Data;

        //[HorizontalLine(2, EColor.Orange)]
        //public bool DebugMode = true;
    }
    public enum LevelCompletedStatus
    {
        Loop,
        FullRandomness,
        RegularRandomness,
        LastLevel
    }
}


