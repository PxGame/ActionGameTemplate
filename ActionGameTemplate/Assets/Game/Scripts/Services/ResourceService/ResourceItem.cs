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
    public class ResourceItem : MonoBehaviour, IResourceItem
    {
        [SerializeField]
        private string _resourceTag;

        public string poolTag => _resourceTag;

        public bool inPool { get; set; }

        public virtual void OnPopPool()
        {
        }

        public virtual void OnPushPool()
        {
        }
    }
}