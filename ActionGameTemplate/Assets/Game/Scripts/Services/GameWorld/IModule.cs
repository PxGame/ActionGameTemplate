/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/27 12:05:52
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// IModule
    /// </summary>
    public interface IModule
    {
        GameWorld gw { get; set; }

        void Initialize();

        void Destory();

        void LogicUpdate();

        void ViewUpdate();
    }
}