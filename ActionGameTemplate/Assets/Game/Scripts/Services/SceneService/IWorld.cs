/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2020/10/30 18:47:06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// IWorld
    /// </summary>
    public interface IWorld
    {
        IEnumerator Initialize();

        IEnumerator UnInitialize();
    }
}