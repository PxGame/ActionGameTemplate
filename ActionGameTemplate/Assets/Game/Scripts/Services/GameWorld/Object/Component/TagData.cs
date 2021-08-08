/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/8/2 0:26:21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    public enum EntityTag
    {
        None = 0b0,
        Player = 0b1,
    }

    /// <summary>
    /// TagData
    /// </summary>
    public class TagData : IComponentData
    {
        public EntityTag value;

        public void Reset()
        {
            value = EntityTag.None;
        }
    }
}