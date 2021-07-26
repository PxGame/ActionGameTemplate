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
using XMLib.Extensions;

namespace AGT
{
    /// <summary>
    /// EntityManager
    /// </summary>
    public class EntityManager : IEnumerable<Entity>
    {
        public static readonly int NoneID = 0;
        private int nextId = 1;

        private LinkedList<Entity> entities = new LinkedList<Entity>();
        private Dictionary<int, LinkedListNode<Entity>> id2node = new Dictionary<int, LinkedListNode<Entity>>();
        private List<int> createIds = new List<int>();
        private List<int> destoryIds = new List<int>();

        public Entity Create()
        {
            Entity entity = ObjectUtility.PopObject<Entity>();
            entity.id = nextId++;

            id2node.Add(entity.id, entities.AddLast(entity));
            createIds.Add(entity.id);

            return entity;
        }

        public void Destory(Entity entity)
        {
            if ((entity.status & EntityStatus.Destoryed) != 0) { return; }
            entity.status |= EntityStatus.Destoryed;
            destoryIds.Add(entity.id);
        }

        public T Get<T>(int id) where T : Entity
        {
            return id2node.TryGetValue(id, out LinkedListNode<Entity> node) ? node.Value as T : null;
        }

        public void ApplyInitialize()
        {
            if (createIds.Count <= 0) { return; }

            foreach (var id in createIds)
            {
                LinkedListNode<Entity> node = id2node[id];
                Entity entity = node.Value;
                if ((entity.status & EntityStatus.Inited) == 0)
                {
                    entity.Initialize();
                }
            }
            createIds.Clear();
        }

        public void ApplyDestory()
        {
            if (destoryIds.Count <= 0) { return; }

            foreach (var id in destoryIds)
            {
                LinkedListNode<Entity> node = id2node[id];
                Entity entity = node.Value;
                if ((entity.status & EntityStatus.Destoryed) != 0)
                {
                    if ((entity.status & EntityStatus.Inited) != 0)
                    {
                        entity.Destory();
                    }

                    id2node.Remove(entity.id);
                    entities.Remove(node);

                    ObjectUtility.PushObject(entity);
                }
            }
            destoryIds.Clear();
        }

        #region IEnumerable<Entity>

        public IEnumerator<Entity> GetEnumerator() => entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => entities.GetEnumerator();

        #endregion IEnumerable<Entity>
    }

    public static class ObjectUtility
    {
        private static MultiTypeObjectPool<IComponentData> _componentPool;
        private static MultiTypeObjectPool<Entity> _entityPool;

        static ObjectUtility()
        {
            _componentPool = new MultiTypeObjectPool<IComponentData>()
            {
                onNew = t => t.Reset(),
                onPush = t => t.Reset()
            };
            _entityPool = new MultiTypeObjectPool<Entity>()
            {
                onNew = t => t.Reset(),
                onPush = t => t.Reset()
            };
        }

        public static T PopComponent<T>() where T : class, IComponentData, new() => _componentPool.Pop<T>();

        public static void PushComponent<T>(T component) where T : class, IComponentData => _componentPool.Push(component);

        public static T PopObject<T>() where T : Entity, new() => _entityPool.Pop<T>();

        public static void PushObject<T>(T entity) where T : Entity => _entityPool.Push(entity);
    }
}