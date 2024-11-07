using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

namespace Core
{
    public class Pool<T> : Pool where T : IPoolable
    {
        [SerializeField]
        private int _numOfInitObj;
        [SerializeField]
        private List<T> _objectList;
        private List<T> _activeList;
        private List<T> _inactiveList;

        [SerializeField]
        private bool _overrideParent = false;

        [SerializeField, ShowIf("_overrideParent")]
        private GameObject _parentForActiveObj, _parentForInactiveObj;

        public override void Initialize()
        {
            _activeList = new List<T>();
            _inactiveList = new List<T>();

            if (!_overrideParent)
            {
                CreateParent();
            }

            for (int i = 0; i < _numOfInitObj; i++)
            {
                var data = CreatePoolPrefab();
                PushForInactivation(data);
            }
        }
        private void CreateParent()
        {
            _parentForActiveObj = new GameObject("ParentForActiveObj");
            _parentForInactiveObj = new GameObject("ParentForInactiveObj");

            _parentForActiveObj.transform.SetParent(transform);
            _parentForInactiveObj.transform.SetParent(transform);
        }
        private T CreatePoolPrefab()
        {
            var random = Random.Range(0, _objectList.Count);

            IPoolable obj = _objectList[random];
            var inst = Instantiate((Object)obj, _parentForInactiveObj.transform) as IPoolable;

            T data = (T)inst;
            data.Create();

            return data;
        }
        private void PushForInactivation(T item)
        {
            _inactiveList.Add(item);
            item.Inactive();
            item.GetGameObject().SetActive(false);
            item.GetTransform().SetParent(_parentForInactiveObj.transform);
        }
        private T PopForActivation()
        {
            int count = _inactiveList.Count;

            if (count > 0)
            {
                var d = _inactiveList.First();
                _inactiveList.Remove(d);

                ActivationOperation(d);
                return d;
            }
            else
            {
                var d = CreatePoolPrefab();
                ActivationOperation(d);

                return d;
            }
        }
        private void ActivationOperation(T item)
        {
            item.GetGameObject().SetActive(true);
            item.Active();
            item.GetTransform().SetParent(_parentForActiveObj.transform);

            _activeList.Add(item);
        }

        public T GetFromPool()
        {
            return PopForActivation();
        }
        public void PutToPool(T data)
        {
            _activeList.Remove(data);
            PushForInactivation(data);
        }
        public override void Clear()
        {
            foreach (var item in _activeList)
                PushForInactivation(item);

            _activeList.Clear();
        }
    }
    public abstract class Pool : MonoBehaviour 
    {
        public abstract void Initialize();
        public abstract void Clear();

        private void Awake()
        {
            var pm=ServiceManager.Instance.GetCoreManager<PoolManager>();
            pm.Register(GetType(),this);
        }
    }
}
