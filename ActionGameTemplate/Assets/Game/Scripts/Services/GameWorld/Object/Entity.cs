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
    [Flags]
    public enum EntityStatus
    {
        None = 0b0,
        Inited = 0b1,
        Destoryed = 0b10,
    }

    /// <summary>
    /// Entity
    /// </summary>
    public class Entity
    {
        public int id;

        public EntityStatus status;

        public Dictionary<Type, IComponentData> components = new Dictionary<Type, IComponentData>();

        public T GetOrAddComponent<T>() where T : class, IComponentData, new()
        {
            return GetComponent<T>() ?? AddComponent<T>();
        }

        public bool HasComponent<T>() where T : class, IComponentData, new()
        {
            return components.ContainsKey(typeof(T));
        }

        public T AddComponent<T>() where T : class, IComponentData, new()
        {
            T data = ObjectUtility.PopComponent<T>();
            components.Add(typeof(T), data);
            return data;
        }

        public T AddComponent<T>(T component) where T : class, IComponentData, new()
        {
            components.Add(component.GetType(), component);
            return component;
        }

        public T GetComponent<T>() where T : class, IComponentData, new()
        {
            return components.TryGetValue(typeof(T), out IComponentData data) ? (T)data : null;
        }

        public void RemoveComponent<T>() where T : class, IComponentData, new()
        {
            if (components.TryGetValue(typeof(T), out IComponentData data))
            {
                components.Remove(typeof(T));
                ObjectUtility.PushComponent((T)data);
            }
        }

        public virtual void Initialize()
        {
        }

        public virtual void Destory()
        {
            foreach (var cmpPair in components)
            {
                ObjectUtility.PushComponent(cmpPair.Value);
            }
            components.Clear();
        }

        public virtual void Reset()
        {
            id = EntityManager.NoneID;
            status = EntityStatus.None;
            components.Clear();
        }
    }

    public interface IComponentData
    {
        void Reset();
    }
}