using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        [SerializeField]
        private List<UIItem> _uiItemList;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        public virtual void InitializePanels()
        {
            foreach (var item in _uiItemList)
                item.InitializeUIItems();

            ClosePanel();
        }
        public T FindItem<T>() where T : UIItem
        {
            foreach (var item in _uiItemList)
                if (item is T uiItem)
                    return uiItem;
            return null;
        }

        public void OpenPanel()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        public void ClosePanel()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
