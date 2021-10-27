/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/10/26 2:06:21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// ResourceItem
    /// </summary>
    [DisallowMultipleComponent]
    public class ResourceItem : MonoBehaviour, IResourceItem
    {
        [SerializeField]
        private string _resourceTag;

        public string poolTag => _resourceTag;

        public bool inPool { get; set; }

        private List<ISubPoolCallback> subPoolCallbacks = new List<ISubPoolCallback>();

        public virtual void Awake()
        {
            GetComponents<ISubPoolCallback>(subPoolCallbacks);
        }

        public virtual void OnPopPool()
        {
            foreach (var callback in subPoolCallbacks)
            {
                callback.OnPopPool();
            }
        }

        public virtual void OnPushPool()
        {
            foreach (var callback in subPoolCallbacks)
            {
                callback.OnPushPool();
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_resourceTag))
            {
                _resourceTag = gameObject.name;
            }
        }

#endif
    }

    public interface ISubPoolCallback : IPoolCallback { }
}