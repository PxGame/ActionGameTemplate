/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/7/25 13:24:50
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// ViewData
    /// </summary>
    public class ViewData : IComponentData
    {
        public string resourceTag;

        public void Reset()
        {
            resourceTag = string.Empty;
        }
    }
}