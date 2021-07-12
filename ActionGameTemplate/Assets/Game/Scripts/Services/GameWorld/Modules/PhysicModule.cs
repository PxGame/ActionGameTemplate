/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:25:22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// PhysicModule
    /// </summary>
    public class PhysicModule : IModule
    {
        public void Destory()
        {
            SuperLog.Log("PhysicModule Destory");
        }

        public void Initialize(GameWorld gw)
        {
            SuperLog.Log("PhysicModule Initialize");
        }

        public void LogicUpdate()
        {
            SuperLog.Log("PhysicModule LogicUpdate");
        }

        public void ViewUpdate()
        {
            SuperLog.Log("PhysicModule ViewUpdate");
        }
    }
}