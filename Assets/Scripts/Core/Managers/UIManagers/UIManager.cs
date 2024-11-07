using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class UIManager : Service
    {
        [SerializeField]
        private List<UIPanel> _panelList;

        public override IEnumerator Initialize()
        {
            InitializeUI();
            yield return _waitForEndOfFrame;
        }
        private void InitializeUI()
        {
            foreach (var item in _panelList)
                item.InitializePanels();
        }
        public T FindPanel<T>() where T:UIPanel
        {
            foreach (var item in _panelList)
                if (item is T panel)
                    return panel;

            return null;
        }
        public K FindItem<T,K>() where T : UIPanel where K:UIItem
        {
            foreach (var item in _panelList)
                if (item is T panel)
                    return panel.FindItem<K>();

            return null;
        }
        public void OpenPanel<T>() where T : UIPanel
        {
            FindPanel<T>().OpenPanel();
        }
        public void ClosePanel<T>() where T : UIPanel
        {
            FindPanel<T>().ClosePanel();
        }
        public void OpenUIItem<T, K>() where T : UIPanel where K : UIItem
        {
            FindItem<T, K>().OpenItem();
        }
        public void CloseUIItem<T, K>() where T : UIPanel where K : UIItem
        {
            FindItem<T, K>().CloseItem();
        }
    }
}

