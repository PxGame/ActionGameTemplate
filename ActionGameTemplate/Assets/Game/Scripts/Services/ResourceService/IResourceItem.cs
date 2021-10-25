/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/10/26 2:01:32
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// IResourceItem
    /// </summary>
    public interface IResourceItem : IPoolItem<string>
    {
        Transform transform { get; }
        GameObject gameObject { get; }
    }
}