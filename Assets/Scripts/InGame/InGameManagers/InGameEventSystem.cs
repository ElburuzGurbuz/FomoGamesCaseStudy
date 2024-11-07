using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InGame
{
    public class InGameEventSystem : InGameService
    {
        [SerializeField]
        private UnityEvent _winEvents;

        [SerializeField]
        private UnityEvent _failEvent;

        public override IEnumerator Initialize()
        {
            yield return _waitForEndFrame;
        }

        public void AddManuelWinEvent(UnityAction action)
        {
            _winEvents.AddListener(action);
        }
        public void AddManuelFailEvent(UnityAction action)
        {
            _failEvent.AddListener(action);
        }
        public void RemoveManuelWinEvent(UnityAction action)
        {
            _winEvents.RemoveListener(action);
        }
        public void RemoveManuelFailEvent(UnityAction action)
        {
            _failEvent.RemoveListener(action);
        }

        public void CallWinEvents()
        {
            _winEvents?.Invoke();
        }

        public void CallFailEvents()
        {
            _failEvent?.Invoke();
        }
    }
}
