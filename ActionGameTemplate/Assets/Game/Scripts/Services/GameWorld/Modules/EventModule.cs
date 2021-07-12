/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:25:12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// EventModule
    /// </summary>
    public class EventModule : IModule
    {
        public void Destory()
        {
            SuperLog.Log("EventModule Destory");
        }

        public void Initialize(GameWorld gw)
        {
            SuperLog.Log("EventModule Initialize");
        }

        public void LogicUpdate()
        {
            SuperLog.Log("EventModule LogicUpdate");
        }

        public void ViewUpdate()
        {
            SuperLog.Log("EventModule ViewUpdate");
        }
    }
}