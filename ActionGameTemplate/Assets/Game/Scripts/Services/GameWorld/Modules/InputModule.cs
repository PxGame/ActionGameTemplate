/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:24:53
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// InputModule
    /// </summary>
    public class InputModule : IModule
    {
        public void Destory()
        {
            SuperLog.Log("InputModule Destory");
        }

        public void Initialize(GameWorld gw)
        {
            SuperLog.Log("InputModule Initialize");
        }

        public void LogicUpdate()
        {
            SuperLog.Log("InputModule LogicUpdate");
        }

        public void ViewUpdate()
        {
            SuperLog.Log("InputModule ViewUpdate");
        }
    }
}