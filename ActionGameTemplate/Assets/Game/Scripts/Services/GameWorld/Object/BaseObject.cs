/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/6 0:18:54
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// BaseObject
    /// </summary>
    public class BaseObject
    {
        public int id;
        public bool isInited;
        public bool isDestoryed;

        private Dictionary<Type, IComponentData> _componentDict = new Dictionary<Type, IComponentData>();

        public bool HasComponent<T>() where T : class, IComponentData, new()
        {
            return _componentDict.ContainsKey(typeof(T));
        }

        public T AddComponent<T>() where T : class, IComponentData, new()
        {
            T data = ObjectUtility.PopComponent<T>();
            _componentDict.Add(typeof(T), data);
            return data;
        }

        public T GetComponent<T>() where T : class, IComponentData, new()
        {
            return _componentDict.TryGetValue(typeof(T), out IComponentData data) ? (T)data : null;
        }

        public void RemoveComponent<T>() where T : class, IComponentData, new()
        {
            if (_componentDict.TryGetValue(typeof(T), out IComponentData data))
            {
                _componentDict.Remove(typeof(T));
                ObjectUtility.PushComponent((T)data);
            }
        }

        public virtual void Initialize()
        {
            isInited = true;
        }

        public virtual void Destory()
        {
            foreach (var cmpPair in _componentDict)
            {
                ObjectUtility.PushComponent(cmpPair.Value);
            }
            _componentDict.Clear();
        }

        public virtual void Reset()
        {
            id = ObjectManager.NoneID;
            isInited = false;
            isDestoryed = false;
        }
    }

    public interface IComponentData
    {
        void Reset();
    }
}