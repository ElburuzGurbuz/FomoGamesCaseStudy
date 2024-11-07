using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class SoundManager : Service
    {
        private bool _isActive;

        public override IEnumerator Initialize()
        {
            yield return _waitForEndOfFrame;
        }

        public void SetActivation(bool active)
        {
            _isActive = active;
        }
    }
}
