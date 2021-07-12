/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/6/28 0:25:31
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib;

namespace AGT
{
    /// <summary>
    /// ViewModule
    /// </summary>
    public class ViewModule : IModule
    {
        public void Destory()
        {
            SuperLog.Log("ViewModule Destory");
        }

        public void Initialize(GameWorld gw)
        {
            SuperLog.Log("ViewModule Initialize");
        }

        public void LogicUpdate()
        {
            SuperLog.Log("ViewModule LogicUpdate");
        }

        public void ViewUpdate()
        {
            SuperLog.Log("ViewModule ViewUpdate");
        }
    }
}