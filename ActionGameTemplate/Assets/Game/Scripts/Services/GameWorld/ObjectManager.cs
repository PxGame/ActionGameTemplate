/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:26:28
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// ObjectManager
    /// </summary>
    public class ObjectManager
    {
        public static readonly int NoneID = 0;
        private int _nextId = 1;

        private LinkedList<BaseObject> objs = new LinkedList<BaseObject>();
        private Dictionary<int, BaseObject> id2obj = new Dictionary<int, BaseObject>();

        public T Create<T>() where T : BaseObject, new()
        {
            T obj = ObjectUtility.PopObject<T>();
            obj.id = _nextId++;
            id2obj[obj.id] = obj;
            return obj;
        }

        public void Destory<T>(T obj) where T : BaseObject
        {
            obj.isDestoryed = true;
        }

        public T Get<T>(int id) where T : BaseObject
        {
            return id2obj.TryGetValue(id, out BaseObject obj) ? obj as T : null;
        }

        public void ApplyInitialize()
        {
            foreach (var obj in objs)
            {
                if (!obj.isInited)
                {
                    obj.Initialize();
                }
            }
        }

        public void ApplyDestory()
        {
            foreach (var obj in objs)
            {
                if (obj.isDestoryed)
                {
                    if (obj.isInited)
                    {
                        obj.Destory();
                    }
                    objs.Remove(obj);
                    id2obj.Remove(obj.id);
                }
            }
        }
    }

    public static class ObjectUtility
    {
        private static MultiTypeObjectPool<IComponentData> _componentPool;
        private static MultiTypeObjectPool<BaseObject> _objPool;

        static ObjectUtility()
        {
            _componentPool = new MultiTypeObjectPool<IComponentData>()
            {
                onNew = t => t.Reset(),
                onPush = t => t.Reset()
            };
            _objPool = new MultiTypeObjectPool<BaseObject>()
            {
                onNew = t => t.Reset(),
                onPush = t => t.Reset()
            };
        }

        public static T PopComponent<T>() where T : class, IComponentData, new() => _componentPool.Pop<T>();

        public static void PushComponent<T>(T obj) where T : class, IComponentData => _componentPool.Push(obj);

        public static T PopObject<T>() where T : BaseObject, new() => _objPool.Pop<T>();

        public static void PushObject<T>(T obj) where T : BaseObject => _objPool.Push(obj);
    }
}