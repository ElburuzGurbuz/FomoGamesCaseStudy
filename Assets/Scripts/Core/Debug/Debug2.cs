using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Core
{
    public static class Debug2
    {
        private static StringBuilder _stringBuilder = new StringBuilder();

        //----------------------------------------------------------------------------
        public static void Log(object f1, int size = 12)
        {
            string str = ColorForDebug2.Pink;
            str = Append("<color=", str, ">", f1.ToString(), "</color></size>");
            Handle(str, size);
        }
        public static void Log(object f1, object f2, int size = 12)
        {
            string str = ColorForDebug2.Pink;
            str = Append("<color=", str, ">", f1.ToString(), " ", f2.ToString(), "</color></size>");
            Handle(str, size);
        }
        public static void Log(object f1, object f2, object f3, int size = 12)
        {
            string str = ColorForDebug2.Pink;
            str = Append("<color=", str, ">", f1.ToString(), " ", f2.ToString(), " ", f3.ToString(), "</color></size>");
            Handle(str, size);
        }

        //----------------------------------------------------------------------------
        public static void Log(object f1, ColorForDebug2 color, int size = 12)
        {
            string str = color;
            str = $"<color={str}>{f1}</color></size>";
            Handle(str, size);
        }
        public static void Log(object f1, object f2, ColorForDebug2 color, int size = 12)
        {
            string str = color;
            str = Append("<color=", str, ">", f1.ToString(), " ", f2.ToString(), "</color></size>");
            Handle(str, size);
        }
        public static void Log(object f1, object f2, object f3, ColorForDebug2 color, int size = 12)
        {
            string str = color;
            str = Append("<color=", str, ">", f1.ToString(), " ", f2.ToString(), " ", f3.ToString(), "</color></size>");
            Handle(str, size);
        }

        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------

        public static void LogEditor(object f1, int size = 12)
        {
#if UNITY_EDITOR
            Log(f1, size);
#endif
        }
        public static void LogEditor(object f1, object f2, int size = 12)
        {
#if UNITY_EDITOR
            Log(f1, f2, size);
#endif
        }
        public static void LogEditor(object f1, object f2, object f3, int size = 12)
        {
#if UNITY_EDITOR
            Log(f1, f2, f3, size);
#endif
        }

        //----------------------------------------------------------------------------
        public static void LogEditor(object f1, ColorForDebug2 color, int size = 12)
        {
#if UNITY_EDITOR
            Log(f1, color, size);
#endif
        }
        public static void LogEditor(object f1, object f2, ColorForDebug2 color, int size = 12)
        {
#if UNITY_EDITOR
            Log(f1, f2, color, size);
#endif
        }
        public static void LogEditor(object f1, object f2, object f3, ColorForDebug2 color, int size = 12)
        {
#if UNITY_EDITOR
            Log(f1, f2, f3, color, size);
#endif
        }
        //----------------------------------------------------------------------------

        public static void Handle(string text, int size)
        {
            var dl = $"<size={size}>";
            dl = $"{dl}{text}";
            Debug.Log(dl);
        }

        private static string Append(params string[] textArray)
        {
            _stringBuilder.Clear();

            foreach (var item in textArray)
                _stringBuilder.Append(item);

            return _stringBuilder.ToString();
        }
    }
    public class ColorForDebug2
    {
        public static implicit operator string(ColorForDebug2 tlc)
        {
            return tlc.Value;
        }

        private ColorForDebug2(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static ColorForDebug2 Aqua { get { return new ColorForDebug2("#00FFFF"); } }
        public static ColorForDebug2 Blue { get { return new ColorForDebug2("#0000FF"); } }
        public static ColorForDebug2 DarkBlue { get { return new ColorForDebug2("#00008B"); } }
        public static ColorForDebug2 DarkGray { get { return new ColorForDebug2("#A9A9A9"); } }
        public static ColorForDebug2 DarkMagenta { get { return new ColorForDebug2("#8B008B"); } }
        public static ColorForDebug2 DarkOrange { get { return new ColorForDebug2("#FF8C00"); } }
        public static ColorForDebug2 DarkRed { get { return new ColorForDebug2("#8B0000"); } }
        public static ColorForDebug2 DarkPink { get { return new ColorForDebug2("#FF1493"); } }
        public static ColorForDebug2 Gold { get { return new ColorForDebug2("#FFD700"); } }
        public static ColorForDebug2 Gray { get { return new ColorForDebug2("#808080"); } }
        public static ColorForDebug2 Grey { get { return new ColorForDebug2("#808080"); } }
        public static ColorForDebug2 Pink { get { return new ColorForDebug2("#FF69B4"); } }
        public static ColorForDebug2 Green { get { return new ColorForDebug2("#008000"); } }
        public static ColorForDebug2 Orange { get { return new ColorForDebug2("#FFA500"); } }
        public static ColorForDebug2 Red { get { return new ColorForDebug2("#FF0000"); } }
        public static ColorForDebug2 White { get { return new ColorForDebug2("#FFFFFF"); } }
        public static ColorForDebug2 Yellow { get { return new ColorForDebug2("#FFFF00"); } }
        public static ColorForDebug2 Black { get { return new ColorForDebug2("#000000"); } }

    }
}
