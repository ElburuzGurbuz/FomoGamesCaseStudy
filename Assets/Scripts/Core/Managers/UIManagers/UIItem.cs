using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIItem : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        public virtual void InitializeUIItems()
        {

        }

        public void OpenItem()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
        }
        public void CloseItem()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
        }
    }
}
