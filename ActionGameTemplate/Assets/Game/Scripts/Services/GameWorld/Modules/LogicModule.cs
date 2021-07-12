/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:25:02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// LogicModule
    /// </summary>
    public class LogicModule : IModule
    {
        public void Destory()
        {
            SuperLog.Log("LogicModule Destory");
        }

        public void Initialize(GameWorld gw)
        {
            SuperLog.Log("LogicModule Initialize");
        }

        public void LogicUpdate()
        {
            SuperLog.Log("LogicModule LogicUpdate");
        }

        public void ViewUpdate()
        {
            SuperLog.Log("LogicModule ViewUpdate");
        }
    }
}