using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace InGame
{
    public class Save : MonoBehaviour
    {
        public Data<string> StringData;
        public Data<int> IntegerData;
        public Data<float> FloatData;
        public Data<bool> BooleanData;
        public Data<Vector3> Vector3Data;
        public Data<Vector2> Vector2Data;

        [HorizontalLine]
        public UnityEvent<int> CallbackInt;
        public UnityEvent<float> CallbackFloat;
        public UnityEvent<string> CallbackString;
        public UnityEvent<bool> CallbackBoolean;
        public UnityEvent<Vector3> CallbackVector3;
        public UnityEvent<Vector2> CallbackVector2;
        
        public void SetData<T>(T value)
        {
            if (typeof(T) == typeof(bool))
            {
                var b = (bool)(object)value;
                BooleanData.V = b;
                CallbackBoolean?.Invoke(b);
            }

            if (typeof(T) == typeof(int))
            {
                var i = (int)(object)value;
                IntegerData.V = i;
                CallbackInt?.Invoke(i);
            }
               
            if (typeof(T) == typeof(float))
            {
                var f = (float)(object)value;
                FloatData.V = f;
                CallbackFloat?.Invoke(f);
            }

            if (typeof(T) == typeof(string))
            {
                var s = (string)(object)value;
                StringData.V = s;
                CallbackString?.Invoke(s);
            }

            if (typeof(T) == typeof(Vector3))
            {
                var v3 = (Vector3)(object)value;
                Vector3Data.V = v3;
                CallbackVector3?.Invoke(v3);
            }

            if (typeof(T) == typeof(Vector2))
            {
                var v2 = (Vector2)(object)value;
                Vector2Data.V = v2;
                CallbackVector2?.Invoke(v2);
            }
        }
    }
}
