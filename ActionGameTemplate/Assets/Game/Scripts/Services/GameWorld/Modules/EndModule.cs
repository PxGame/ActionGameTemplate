/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:24:42
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// EndModule
    /// </summary>
    public class EndModule : IModule
    {
        public void Destory()
        {
            SuperLog.Log("EndModule Destory");
        }

        public void Initialize(GameWorld gw)
        {
            SuperLog.Log("EndModule Initialize");
        }

        public void LogicUpdate()
        {
            SuperLog.Log("EndModule LogicUpdate");
        }

        public void ViewUpdate()
        {
            SuperLog.Log("EndModule ViewUpdate");
        }
    }
}