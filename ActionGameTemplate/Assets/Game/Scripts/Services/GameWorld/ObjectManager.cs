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
        private int nextId = 1;

        private LinkedList<BaseObject> objs = new LinkedList<BaseObject>();
        private Dictionary<int, LinkedListNode<BaseObject>> id2node = new Dictionary<int, LinkedListNode<BaseObject>>();
        private List<int> createIds = new List<int>();
        private List<int> destoryIds = new List<int>();

        public T Create<T>() where T : BaseObject, new()
        {
            T obj = ObjectUtility.PopObject<T>();
            obj.id = nextId++;

            id2node[obj.id] = objs.AddLast(obj);

            createIds.Add(obj.id);

            return obj;
        }

        public void Destory<T>(T obj) where T : BaseObject
        {
            obj.isDestoryed = true;

            destoryIds.Add(obj.id);
        }

        public T Get<T>(int id) where T : BaseObject
        {
            return id2node.TryGetValue(id, out LinkedListNode<BaseObject> node) ? node.Value as T : null;
        }

        public void ApplyInitialize()
        {
            if (createIds.Count <= 0) { return; }

            foreach (var id in createIds)
            {
                LinkedListNode<BaseObject> node = id2node[id];
                BaseObject obj = node.Value;
                if (!obj.isInited)
                {
                    obj.Initialize();
                }
            }
            createIds.Clear();
        }

        public void ApplyDestory()
        {
            if (destoryIds.Count <= 0) { return; }

            foreach (var id in destoryIds)
            {
                LinkedListNode<BaseObject> node = id2node[id];
                BaseObject obj = node.Value;
                if (obj.isDestoryed)
                {
                    if (obj.isInited)
                    {
                        obj.Destory();
                    }

                    id2node.Remove(obj.id);
                    objs.Remove(node);
                }
            }
            destoryIds.Clear();
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