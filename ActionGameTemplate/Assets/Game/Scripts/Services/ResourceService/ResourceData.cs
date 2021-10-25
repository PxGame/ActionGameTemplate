/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/10/26 2:44:14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// ResourceData
    /// </summary>
    [System.Serializable]
    public class ResourceData
    {
        public Dictionary<string, string> tag2path = new Dictionary<string, string>();
    }
}