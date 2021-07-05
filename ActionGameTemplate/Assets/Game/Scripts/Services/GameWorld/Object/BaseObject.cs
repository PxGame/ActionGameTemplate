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
        public static readonly int NoneID = 0;

        public int id;

        private Dictionary<Type, IComponentData> _componentDict = new Dictionary<Type, IComponentData>();

        public bool HasComponent<T>() where T : IComponentData, new()
        {
            return _componentDict.ContainsKey(typeof(T));
        }

        public T AddComponent<T>() where T : IComponentData, new()
        {
            T data = ComponentUtility.Pop<T>();
            _componentDict.Add(typeof(T), data);
            return data;
        }

        public T GetComponent<T>() where T : IComponentData, new()
        {
            return _componentDict.TryGetValue(typeof(T), out IComponentData data) ? (T)data : default(T);
        }

        public void RemoveComponent<T>() where T : IComponentData, new()
        {
            if (_componentDict.TryGetValue(typeof(T), out IComponentData data))
            {
                _componentDict.Remove(typeof(T));
                ComponentUtility.Push((T)data);
            }
        }

        public virtual void Initialize()
        {
        }

        public virtual void Destory()
        {
            foreach (var cmpPair in _componentDict)
            {
                ComponentUtility.Push(cmpPair.Value);
            }
            _componentDict.Clear();

            id = NoneID;
        }
    }

    public interface IComponentData
    {
        void Reset();
    }

    public static class ComponentUtility
    {
        private static Dictionary<Type, Stack<IComponentData>> _pool = new Dictionary<Type, Stack<IComponentData>>();

        public static T Pop<T>() where T : IComponentData, new()
        {
            if (!_pool.TryGetValue(typeof(T), out Stack<IComponentData> datas))
            {
                datas = new Stack<IComponentData>();
                _pool[typeof(T)] = datas;
            }

            T result;
            if (datas.Count > 0)
            {
                result = (T)datas.Pop();
            }
            else
            {
                result = new T();
                result.Reset();
            }

            return result;
        }

        public static void Push<T>(T data) where T : IComponentData
        {
            if (!_pool.TryGetValue(typeof(T), out Stack<IComponentData> datas))
            {
                datas = new Stack<IComponentData>();
                _pool[typeof(T)] = datas;
            }
            data.Reset();
            datas.Push(data);
        }
    }
}